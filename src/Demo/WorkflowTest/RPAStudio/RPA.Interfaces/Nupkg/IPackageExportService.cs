using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Nupkg
{
    public interface IPackageExportService
    {
        void Init(string id, string version, string description, string authors, string owners, string releaseNotes);

        void WithDependencies(IEnumerable<NugetPackageItem> dependencies);

        void WithFiles(string projectPath, string exclude);

        string ExportToDir(string nupkgLocation);


    }
}
