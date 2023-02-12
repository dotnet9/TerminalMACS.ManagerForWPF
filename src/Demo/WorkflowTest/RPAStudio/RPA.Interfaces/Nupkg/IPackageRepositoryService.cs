using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Nupkg
{
    public interface IPackageRepositoryService
    {
        void Init(string packageSource);

        List<NugetPackageItem> GetMatchedPackagesByIdAndMaxVersion(string matchRegex);
    }
}
