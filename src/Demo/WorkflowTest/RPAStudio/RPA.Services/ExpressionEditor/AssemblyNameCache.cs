using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public static class AssemblyNameCache
    {
        private static readonly ConcurrentDictionary<string, string> _assemblyShortNames = new ConcurrentDictionary<string, string>();

        public static string GetCachedShortName(this Assembly assembly)
        {
            if (!(assembly == null))
            {
                return _assemblyShortNames.GetOrAdd(assembly.FullName, (string _) => assembly.GetName().Name);
            }
            return null;
        }

        public static void Invalidate()
        {
            _assemblyShortNames.Clear();
        }
    }

}
