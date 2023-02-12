using ActiproSoftware.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.ExpressionEditor
{
    public enum ExpressionLanguage
    {
        VisualBasic,
    }

    public interface ISyntaxService
    {
        void AddAssemblyReferences(IEnumerable<string> assemblyNames);

        void AddAssemblyReferences(IEnumerable<Assembly> assemblies);

        Task<ISyntaxLanguage> GetLanguageAsync(ExpressionLanguage expresionLanguage);
    }
}
