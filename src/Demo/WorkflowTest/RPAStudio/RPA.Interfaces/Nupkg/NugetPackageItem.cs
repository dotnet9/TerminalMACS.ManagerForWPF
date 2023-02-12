using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Nupkg
{
    public class NugetPackageItem
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public NugetPackageItem(string id, string version)
        {
            this.Id = id;
            this.Version = version;
        }
    }
}
