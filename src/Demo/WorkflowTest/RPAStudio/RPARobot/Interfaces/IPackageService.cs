using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPARobot.Interfaces
{
    public interface IPackageService
    {
        void Run(string name, string version);
    }
}
