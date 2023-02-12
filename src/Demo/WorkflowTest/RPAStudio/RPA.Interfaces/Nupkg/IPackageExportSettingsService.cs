using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Nupkg
{
    public interface IPackageExportSettingsService
    {
        string GetLastExportDir();
        bool SetLastExportDir(string dir);

        List<string> GetExportDirHistoryList();
        bool AddToExportDirHistoryList(string dir);
    }
}
