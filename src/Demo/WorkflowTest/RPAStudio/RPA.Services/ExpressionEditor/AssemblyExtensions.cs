using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public static class AssemblyExtensions
    {
        public static bool IsBrowsable(this Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            BrowsableAttribute attribute;
            if (TryGetAttribute(assembly, out attribute))
            {
                return attribute.Browsable;
            }
            return true;
        }

        private static bool TryGetAttribute<T>(this Assembly assembly, out T attribute) where T : Attribute
        {
            attribute = assembly.GetCustomAttributes(typeof(T), inherit: false).Cast<T>().FirstOrDefault();
            return attribute != null;
        }
    }

}
