using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Activities
{
    public interface IActivitiesService
    {
        List<string> Assemblies { get; }

        List<string> CustomActivityConfigXmls { get; }

        List<string> Init(List<string> assemblies);

        Stream GetIcon(string assemblyName, string relativeResPath);

        string GetAssemblyQualifiedName(string typeOf);

        void SetSharedObjectInstance(object instance);

        bool IsXamlValid(string xamlPath);

        bool IsXamlStringValid(string xamlString);
    }
}
