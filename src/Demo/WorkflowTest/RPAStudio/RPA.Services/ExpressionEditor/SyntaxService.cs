using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiproSoftware.Text;
using System.Threading;
using ActiproSoftware.Text.Languages.VB.Implementation;
using ActiproSoftware.Text.Languages.DotNet.Reflection;
using System.Reflection;
using System.Collections.Concurrent;
using Microsoft.VisualBasic.Activities;
using ActiproSoftware.Text.Languages.DotNet.Reflection.Implementation;
using System.Windows;
using RPA.Interfaces.ExpressionEditor;
using ActiproSoftware.Text.Languages.DotNet;

namespace RPA.Services.ExpressionEditor
{
    internal class SyntaxService : ISyntaxService, IDisposable
    {
        private bool _isDisposed;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Lazy<Task<ISyntaxLanguage>> _vbSyntaxLanguageLoadTask;
        private readonly IProjectAssembly _projectAssembly;
        private readonly Lazy<LoadIntellisenseAssembliesContext> _context;
        private readonly ConcurrentDictionary<string, IBinaryAssembly> _loadedIntellisenseAssemblies = new ConcurrentDictionary<string, IBinaryAssembly>();
        private readonly TaskScheduler _serializingScheduler = new ConcurrentExclusiveSchedulerPair().ExclusiveScheduler;
        private string _cacheFolder;
        private readonly IWorkflowImportReferenceService _importsService;

        public static string CacheInFolder { get; set; }

        public SyntaxService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _vbSyntaxLanguageLoadTask = new Lazy<Task<ISyntaxLanguage>>(() => Task.Run(() => CreateVBSyntaxLanguage()));
            _projectAssembly = new VBProjectAssembly("ExpressionEditor");

            _context = new Lazy<LoadIntellisenseAssembliesContext>(LoadIntellisenseAssembliesContext.Create);

            this._cacheFolder = CacheInFolder + @"\.cache";
            AmbientAssemblyRepositoryProvider.Repository = new FileBasedAssemblyRepository(this._cacheFolder);

            _importsService = new WorkflowImportReferenceService();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._cancellationTokenSource.Cancel();
                }
                this._isDisposed = true;
            }
        }


        private ISyntaxLanguage CreateVBSyntaxLanguage()
        {
            try
            {
                VBSyntaxLanguage vBSyntaxLanguage = new VBSyntaxLanguage();
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    return null;
                }

                vBSyntaxLanguage.RegisterProjectAssembly(_projectAssembly);
                vBSyntaxLanguage.RegisterService<VBCompletionProvider>(new CustomVBCompletionProvider());
                return vBSyntaxLanguage;
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception exception)
            {
                exception.Trace("CreateVBSyntaxLanguage");
            }
            return null;
        }



        public Task<ISyntaxLanguage> GetLanguageAsync(ExpressionLanguage expressionLanguage)
        {
            if (this._isDisposed)
            {
                return null;
            }

            return this._vbSyntaxLanguageLoadTask.Value;
        }

        private bool ShouldLoadAssembly(Assembly assembly)
        {
            return assembly != null && !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location);
        }


        private bool ShouldLoadAssembly(string assemblyName)
        {
            return !string.IsNullOrEmpty(assemblyName) && !this._loadedIntellisenseAssemblies.ContainsKey(assemblyName) && !this._context.Value.FailedAssemblies.ContainsKey(assemblyName) && !this._context.Value.DynamicAppdomainAssemblies.Contains(assemblyName) && !assemblyName.EndsWith("ContentType=WindowsRuntime");
        }

        private void AddAssemblyReferencesInternal(List<string> assemblyNames)
        {
            if (assemblyNames != null)
            {
                Application.Current.Dispatcher.InvokeAsync(async delegate
                {
                    try
                    {
                        await LoadDefaultReferencesAsync(_context.Value);
                        await LoadAssembliesAsync(_context.Value, assemblyNames);
                    }
                    catch (Exception exception)
                    {
                        exception.Trace("SyntaxService.AddAssemblyReferences");
                    }
                });
            }
        }



        private async Task LoadAssembliesAsync(LoadIntellisenseAssembliesContext context, List<string> assemblies)
        {
            if (assemblies.Count != 0)
            {
                await Task.Factory.StartNew(delegate ()
                {
                    foreach (string assemblyName in assemblies)
                    {
                        if (this._cancellationTokenSource.IsCancellationRequested)
                        {
                            break;
                        }
                        this.LoadAssembly(context, assemblyName, this._cancellationTokenSource.Token);
                    }
                }, this._cancellationTokenSource.Token, TaskCreationOptions.LongRunning, this._serializingScheduler);
                if (!this._cancellationTokenSource.IsCancellationRequested)
                {
                    this._loadedIntellisenseAssemblies.Values.ForEach(delegate (IBinaryAssembly r)
                    {
                        this.TryPublishResult(r);
                    });
                }
            }
        }

        private void TryPublishResult(IAssembly assembly)
        {
            try
            {
                if (assembly != null)
                {
                    this._projectAssembly.AssemblyReferences.Add(assembly);
                }
            }
            catch (Exception exception)
            {
                exception.Trace("SyntaxService.TryPublishResult, " + ((assembly != null) ? assembly.Name : null));
            }
        }


        public bool IsBrowsable(Assembly assembly)
        {
            if (!assembly.IsDynamic && assembly.IsBrowsable())
            {
                if (!assembly.GlobalAssemblyCache)
                {
                    return true;
                }
                return true;
            }
            return false;
        }

        private void LoadAssembly(LoadIntellisenseAssembliesContext context, string assemblyName, CancellationToken cancellationToken)
        {
            IBinaryAssembly binaryAssembly;
            if (!this._loadedIntellisenseAssemblies.TryGetValue(assemblyName, out binaryAssembly))
            {
                try
                {
                    binaryAssembly = AmbientAssemblyRepositoryProvider.Repository.Load(assemblyName);
                    AmbientAssemblyRepositoryProvider.Repository.Add(binaryAssembly);
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        string key = assemblyName;
                        Assembly assembly;
                        if (context.AppdomainAssembliesByName.TryGetValue(assemblyName, out assembly))
                        {
                            key = assembly.FullName;
                        }
                        this._loadedIntellisenseAssemblies[key] = binaryAssembly;
                        foreach (IBinaryAssemblyReference binaryAssemblyReference in binaryAssembly.AssemblyReferences)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                            Assembly assembly2;
                            if (context.AppdomainAssembliesByName.TryGetValue(binaryAssemblyReference.AssemblyName.Name, out assembly2) && this.IsBrowsable(assembly2))
                            {
                                this.LoadAssembly(context, assembly2.FullName, cancellationToken);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.FailedAssemblies[assemblyName] = ex.Message;
                    ex.Trace("$LoadAssembly: " + assemblyName);
                }
            }
        }



        private async Task LoadDefaultReferencesAsync(LoadIntellisenseAssembliesContext context)
        {
            if (!context.IsDefaultLoaded)
            {
                context.IsDefaultLoaded = true;
                IList<VisualBasicImportReference> assembliesToLoad = this.GetDefaultImports();
                await Task.Run(delegate ()
                {
                    foreach (VisualBasicImportReference visualBasicImportReference in assembliesToLoad)
                    {
                        try
                        {
                            this.LoadAssembly(context, visualBasicImportReference.Assembly, CancellationToken.None);
                        }
                        catch (Exception exception)
                        {
                            exception.Trace("SyntaxService.LoadDefaultReferencesAsync");
                        }
                    }
                });
            }
        }


        private IList<VisualBasicImportReference> GetDefaultImports()
        {
            List<VisualBasicImportReference> list = this._importsService.DefaultSettings.ImportReferences.ToList();
            (from s in list
             where s.Assembly == "system"
             select s).ForEach(delegate (VisualBasicImportReference s)
             {
                 s.Assembly = "System";
             });
            return list;
        }


        public void AddAssemblyReferences(IEnumerable<Assembly> assemblies)
        {
            AddAssemblyReferencesInternal((from asm in assemblies?.Where(ShouldLoadAssembly)
                                           select asm.FullName).ToList());
        }

        public void AddAssemblyReferences(IEnumerable<string> assemblyNames)
        {
            AddAssemblyReferencesInternal(assemblyNames?.Where(ShouldLoadAssembly).ToList());
        }



    }
}
