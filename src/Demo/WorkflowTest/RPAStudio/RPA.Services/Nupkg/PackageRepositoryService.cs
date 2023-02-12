using NuGet;
using RPA.Interfaces.Nupkg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RPA.Services.Nupkg
{
    public class PackageRepositoryService : IPackageRepositoryService
    {
        private IPackageRepository _repo;
        public List<NugetPackageItem> GetMatchedPackagesByIdAndMaxVersion(string matchRegex)
        {
            var ret = new List<NugetPackageItem>();
            var nupkgs = _repo.GetPackages();
            foreach (var item in nupkgs)
            {
                if (Regex.IsMatch(item.Id, matchRegex, RegexOptions.IgnoreCase))
                {
                    var ver = _repo.FindPackagesById(item.Id).Max(p => p.Version);//有多个版本时的排序处理
                    ret.Add(new NugetPackageItem(item.Id, ver.ToString()));
                }
            }

            return ret;
        }

        public void Init(string packageSource)
        {
            _repo = PackageRepositoryFactory.Default.CreateRepository(packageSource);
        }
    }
}
