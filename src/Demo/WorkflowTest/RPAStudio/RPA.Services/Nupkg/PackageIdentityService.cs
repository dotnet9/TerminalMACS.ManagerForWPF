using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;
using RPA.Interfaces.Nupkg;
using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPA.Services.Nupkg
{
    public class PackageIdentityService : IPackageIdentityService
    {
        private PackageIdentity _identity;

        private NugetPackageControl _packageControl;

        public PackageIdentityService()
        {
            _packageControl = new NugetPackageControl();
        }

        public async Task<List<string>> BuildDependentAssemblies(string pkgName, string pkgVer)
        {
            _identity = new PackageIdentity(pkgName, NuGetVersion.Parse(pkgVer));

            var list = await GetDependentAssembliesRecord(_identity);
            list = list.ConvertAll(f => f.Replace(@"/", @"\"));
            return list;
        }


        /// <summary>
        /// 下载和安装一个包
        /// </summary>
        /// <param name="identity">包id</param>
        private async Task<List<string>> GetDependentAssembliesRecord(PackageIdentity identity)
        {
            var retList = new List<string>();
            //包源靠前的地址已经找到和安装了包的时候不要再继续下面的包源操作了
            try
            {
                using (var cacheContext = new SourceCacheContext())
                {
                    var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
                    await _packageControl.GetPackageDependencies(identity, cacheContext, availablePackages);

                    var resolverContext = new PackageResolverContext(
                        DependencyBehavior.Lowest,
                        new[] { identity.Id },
                        Enumerable.Empty<string>(),
                        Enumerable.Empty<NuGet.Packaging.PackageReference>(),
                        Enumerable.Empty<PackageIdentity>(),
                        availablePackages,
                        _packageControl.SourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                        NullLogger.Instance);

                    var resolver = new PackageResolver();
                    var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                        .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
                    var packagePathResolver = new NuGet.Packaging.PackagePathResolver(_packageControl.PackagesInstallFolder);
                    var packageExtractionContext = new PackageExtractionContext(_packageControl.Logger);
                    packageExtractionContext.PackageSaveMode = PackageSaveMode.Defaultv3;
                    var frameworkReducer = new FrameworkReducer();

                    foreach (var packageToInstall in packagesToInstall)
                    {
                        // PackageReaderBase packageReader;
                        var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                        if (installedPath == null)
                        {
                            var downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);
                            var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                                packageToInstall,
                                new PackageDownloadContext(cacheContext),
                                NuGet.Configuration.SettingsUtility.GetGlobalPackagesFolder(_packageControl.Settings),
                                _packageControl.Logger, CancellationToken.None);

                            await PackageExtractor.ExtractPackageAsync(
                                downloadResult.PackageStream,
                                packagePathResolver,
                                packageExtractionContext,
                                CancellationToken.None);
                        }

                        InstallPackage(packageToInstall, ref retList);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, "下载和安装nupkg包过程中出错", ex);
            }

            return retList;
        }

        /// <summary>
        /// 安装指定的包
        /// </summary>
        /// <param name="identity">包id</param>
        /// <returns>是否安装成功</returns>
        public bool InstallPackage(PackageIdentity identity, ref List<string> retList)
        {
            bool ret = true;

            var packagePathResolver = new NuGet.Packaging.PackagePathResolver(_packageControl.PackagesInstallFolder);
            var installedPath = packagePathResolver.GetInstalledPath(identity);

            PackageReaderBase packageReader;
            packageReader = new PackageFolderReader(installedPath);
            var libItems = packageReader.GetLibItems();
            var frameworkReducer = new FrameworkReducer();
            var nearest = frameworkReducer.GetNearest(_packageControl.NuGetFramework, libItems.Select(x => x.TargetFramework));
            var files = libItems
                .Where(x => x.TargetFramework.Equals(nearest))
                .SelectMany(x => x.Items).ToList();
            foreach (var f in files)
            {
                RecordInstallFile(installedPath, f, ref retList);
            }

            var cont = packageReader.GetContentItems();
            nearest = frameworkReducer.GetNearest(_packageControl.NuGetFramework, cont.Select(x => x.TargetFramework));
            files = cont
                .Where(x => x.TargetFramework.Equals(nearest))
                .SelectMany(x => x.Items).ToList();
            foreach (var f in files)
            {
                RecordInstallFile(installedPath, f, ref retList);
            }

            try
            {
                var dependencies = packageReader.GetPackageDependencies();
                nearest = frameworkReducer.GetNearest(_packageControl.NuGetFramework, dependencies.Select(x => x.TargetFramework));
                foreach (var dep in dependencies.Where(x => x.TargetFramework.Equals(nearest)))
                {
                    foreach (var p in dep.Packages)
                    {
                        var local = getLocal(p.Id);
                        InstallPackage(local.Identity, ref retList);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, "安装nupkg包出错", ex);
            }

            if (System.IO.Directory.Exists(installedPath + @"\build"))
            {
                if (System.IO.Directory.Exists(installedPath + @"\build\x64"))
                {
                    foreach (var f in System.IO.Directory.GetFiles(installedPath + @"\build\x64"))
                    {
                        var filename = System.IO.Path.GetFileName(f);
                        recordFile(f, ref retList);
                    }
                }
            }

            return ret;
        }



        /// <summary>
        /// 获取本地包
        /// </summary>
        /// <param name="identity">包id</param>
        /// <returns>本地包</returns>
        private LocalPackageInfo getLocal(string identity)
        {
            FindLocalPackagesResourceV2 findLocalPackagev2 = new FindLocalPackagesResourceV2(_packageControl.PackagesInstallFolder);
            var packages = findLocalPackagev2.GetPackages(_packageControl.Logger, CancellationToken.None).ToList();
            packages = packages.Where(p => p.Identity.Id == identity).ToList();
            LocalPackageInfo res = null;
            foreach (var p in packages)
            {
                if (res == null) res = p;
                if (res.Identity.Version < p.Identity.Version) res = p;
            }

            return res;
        }


        /// <summary>
        /// 安装包文件
        /// </summary>
        /// <param name="installedPath">安装路径</param>
        /// <param name="f">文件</param>
        private void RecordInstallFile(string installedPath, string f, ref List<string> retList)
        {
            string source = "";
            try
            {
                source = System.IO.Path.Combine(installedPath, f);
                recordFile(source, ref retList);
            }
            catch (Exception ex)
            {

            }
        }


        private void recordFile(string source, ref List<string> retList)
        {
            retList.Add(source);
        }


    }
}
