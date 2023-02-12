using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Nupkg
{
    public interface IPackageImportService
    {
        bool Init(string nupkgFile);

        string GetId();
        string GetVersion();
        string GetDescription();

        bool ExtractToDirectory(string path);
    }
}
