using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using RPA.Nupkg.Nupkg;
using RPA.Interfaces.Share;
using RPA.Shared.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RPA.Services.Nupkg
{
    public class NugetPackageControl
    {
        public NugetPackageControl()
        {
            if (!System.IO.Directory.Exists(GlobalPackagesFolder))
            {
                System.IO.Directory.CreateDirectory(GlobalPackagesFolder);
            }

            if (!System.IO.Directory.Exists(PackagesInstallFolder))
            {
                System.IO.Directory.CreateDirectory(PackagesInstallFolder);
            }
        }


        private ILogger _logger = null;
        public ILogger Logger
        {
            get
            {
                if (_logger == null) _logger = NuGetPackageControllerLogger.Instance;
                return _logger;
            }
        }

        public NuGetFramework _nuGetFramework = null;
        public NuGetFramework NuGetFramework
        {
            get
            {
                if (_nuGetFramework == null) _nuGetFramework = NuGetFramework.ParseFolder("net461");
                return _nuGetFramework;
            }
        }

        public NuGet.Configuration.ISettings _settings = null;
        public NuGet.Configuration.ISettings Settings
        {
            get
            {
                var nugetConfigFile = "Nuget.Default.Config";
                try
                {
                    if (_settings == null) _settings = NuGet.Configuration.Settings.LoadSpecificSettings(SharedObject.Instance.ApplicationCurrentDirectory, nugetConfigFile);
                    return _settings;
                }
                catch (Exception err)
                {
                    SharedObject.Instance.Output(SharedObject.enOutputType.Error, $"读取当前目录下的{nugetConfigFile}配置文件出错", err);
                    return null;
                }

            }
        }

        public NuGet.Configuration.ISettings _userSettings = null;
        public NuGet.Configuration.ISettings UserSettings
        {
            get
            {
                var nugetConfigFile = "Nuget.User.Config";
                try
                {
                    if (_userSettings == null) _userSettings = NuGet.Configuration.Settings.LoadSpecificSettings(SharedObject.Instance.ApplicationCurrentDirectory, nugetConfigFile);
                    return _userSettings;
                }
                catch (Exception err)
                {
                    SharedObject.Instance.Output(SharedObject.enOutputType.Error, $"读取当前目录下的{nugetConfigFile}配置文件出错", err);
                    return null;
                }

            }
        }

        /// <summary>
        /// 不缓存，因为有可能中间包源被禁用
        /// </summary>
        public SourceRepositoryProvider SourceRepositoryProvider
        {
            get
            {
                var psp = new PackageSourceProvider(Settings, null, new PackageSourceProvider(UserSettings).LoadPackageSources());
                var _sourceRepositoryProvider = new SourceRepositoryProvider(psp, Repository.Provider.GetCoreV3());

                return _sourceRepositoryProvider;
            }
        }

        private SourceRepositoryProvider _defaultSourceRepositoryProvider = null;
        public SourceRepositoryProvider DefaultSourceRepositoryProvider
        {
            get
            {
                if (_defaultSourceRepositoryProvider == null)
                {
                    var psp = new PackageSourceProvider(Settings);
                    _defaultSourceRepositoryProvider = new SourceRepositoryProvider(psp, Repository.Provider.GetCoreV3());
                }
                return _defaultSourceRepositoryProvider;
            }
        }

        private SourceRepositoryProvider _userDefineSourceRepositoryProvider = null;
        public SourceRepositoryProvider UserDefineSourceRepositoryProvider
        {
            get
            {
                if (_userDefineSourceRepositoryProvider == null)
                {
                    var psp = new PackageSourceProvider(UserSettings);
                    _userDefineSourceRepositoryProvider = new SourceRepositoryProvider(psp, Repository.Provider.GetCoreV3());
                }
                return _userDefineSourceRepositoryProvider;
            }
        }


        public string globalPackagesFolder = null;
        public string GlobalPackagesFolder
        {
            get
            {
                if (string.IsNullOrEmpty(globalPackagesFolder)) globalPackagesFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + $"\\{AppPathConfig.StudioName}\\Packages\\.nuget\\packages";
                return globalPackagesFolder;
            }
        }

        public string _packagesInstallFolder = null;
        public string PackagesInstallFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_packagesInstallFolder)) _packagesInstallFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + $"\\{AppPathConfig.StudioName}\\Packages\\Installed";
                return _packagesInstallFolder;
            }
        }

      
       

        /// <summary>
        /// 获取包依赖
        /// </summary>
        public async Task GetPackageDependencies(PackageIdentity package, SourceCacheContext cacheContext, ISet<SourcePackageDependencyInfo> availablePackages)
        {
            if (availablePackages.Contains(package)) return;

            var repositories = SourceRepositoryProvider.GetRepositories();
            foreach (var sourceRepository in repositories)
            {
                try
                {
                    var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                    var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                        package, NuGetFramework, Logger, CancellationToken.None);
                    if (dependencyInfo == null) continue;
                    availablePackages.Add(dependencyInfo);
                    foreach (var dependency in dependencyInfo.Dependencies)
                    {
                        var identity = new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion);
                        await GetPackageDependencies(identity, cacheContext, availablePackages);
                    }

                    break;//只要有一个源能搜索到就不再往下搜索了
                }
                catch (Exception err)
                {

                }

            }
        }

        /// <summary>
        /// 下载和安装一个包
        /// </summary>
        /// <param name="identity">包id</param>
        public async Task DownloadAndInstall(PackageIdentity identity)
        {
            //包源靠前的地址已经找到和安装了包的时候不要再继续下面的包源操作了
            try
            {
                using (var cacheContext = new SourceCacheContext())
                {
                    var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
                    await GetPackageDependencies(identity, cacheContext, availablePackages);

                    var resolverContext = new PackageResolverContext(
                        DependencyBehavior.Lowest,
                        new[] { identity.Id },
                        Enumerable.Empty<string>(),
                        Enumerable.Empty<NuGet.Packaging.PackageReference>(),
                        Enumerable.Empty<PackageIdentity>(),
                        availablePackages,
                        SourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                        NullLogger.Instance);

                    var resolver = new PackageResolver();
                    var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                        .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
                    var packagePathResolver = new NuGet.Packaging.PackagePathResolver(PackagesInstallFolder);
                    var packageExtractionContext = new PackageExtractionContext(Logger);
                    packageExtractionContext.PackageSaveMode = PackageSaveMode.Defaultv3;
                    var frameworkReducer = new FrameworkReducer();

                    foreach (var packageToInstall in packagesToInstall)
                    {
                        var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                        if (installedPath == null)
                        {
                            var downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);
                            var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                                packageToInstall,
                                new PackageDownloadContext(cacheContext),
                                NuGet.Configuration.SettingsUtility.GetGlobalPackagesFolder(Settings),
                                Logger, CancellationToken.None);

                            await PackageExtractor.ExtractPackageAsync(
                                downloadResult.PackageStream,
                                packagePathResolver,
                                packageExtractionContext,
                                CancellationToken.None);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SharedObject.Instance.Output(SharedObject.enOutputType.Error, "下载和安装nupkg包过程中出错", ex);
            }

        }

        

        /// <summary>
        /// 创建资源提供者
        /// </summary>
        /// <returns>资源提供者</returns>
        public List<Lazy<INuGetResourceProvider>> CreateResourceProviders()
        {
            var result = new List<Lazy<INuGetResourceProvider>>();
            Repository.Provider.GetCoreV3();
            return result;
        }

        /// <summary>
        /// 获取本地包
        /// </summary>
        /// <param name="identity">包id</param>
        /// <returns>本地包</returns>
        private LocalPackageInfo getLocal(string identity)
        {
            FindLocalPackagesResourceV2 findLocalPackagev2 = new FindLocalPackagesResourceV2(PackagesInstallFolder);
            var packages = findLocalPackagev2.GetPackages(Logger, CancellationToken.None).ToList();
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
        /// 获取本地包信息
        /// </summary>
        /// <param name="identity">包id</param>
        /// <returns>包信息</returns>
        public LocalPackageInfo GetLocalPackageInfo(PackageIdentity identity)
        {
            FindLocalPackagesResourceV2 findLocalPackagev2 = new FindLocalPackagesResourceV2(PackagesInstallFolder);
            var packages = findLocalPackagev2.GetPackages(Logger, CancellationToken.None).ToList();
            packages = packages.Where(p => p.Identity.Id == identity.Id).ToList();
            LocalPackageInfo res = null;
            foreach (var p in packages)
            {
                if (p.Identity.Version == identity.Version)
                {
                    res = p;
                    break;
                }
            }
            return res;
        }

  

        /// <summary>
        /// 如果更新时拷贝
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        private void CopyIfNewer(string source, string target)
        {
            var infoOld = new System.IO.FileInfo(source);
            var infoNew = new System.IO.FileInfo(target);
            if (infoNew.LastWriteTime != infoOld.LastWriteTime)
            {
                try
                {
                    System.IO.File.Copy(source, target, true);
                    return;
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// 从安装目录里获取Nuspec读取器
        /// </summary>
        /// <param name="identity">包id</param>
        /// <returns>Nuspec读取器</returns>
        public NuspecReader GetNuspecReaderInPackagesInstallFolder(PackageIdentity identity)
        {
            var packagePathResolver = new NuGet.Packaging.PackagePathResolver(PackagesInstallFolder);
            var installedPath = packagePathResolver.GetInstalledPath(identity);

            PackageReaderBase packageReader;
            packageReader = new PackageFolderReader(installedPath);

            return packageReader.NuspecReader;
        }



    }
}
