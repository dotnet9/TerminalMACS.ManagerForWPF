using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.AppDomains
{
    public interface IAssemblyResolveService
    {
        void Init(List<string> assemblies);
    }
}
