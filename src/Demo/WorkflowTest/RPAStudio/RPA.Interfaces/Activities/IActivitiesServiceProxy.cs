using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RPA.Interfaces.Activities
{
    public interface IActivitiesServiceProxy
    {
        List<string> CustomActivityConfigXmls { get; }

        List<string> Init(List<string> assemblies);

        ImageSource GetIcon(string assemblyName, string relativeResPath);

        void SetSharedObjectInstance();

        string GetAssemblyQualifiedName(string typeOf);

        bool IsXamlValid(string xamlPath);

        bool IsXamlStringValid(string xamlString);
    }
}
