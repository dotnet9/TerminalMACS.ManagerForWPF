using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public class LoadIntellisenseAssembliesContext
    {
        public ConcurrentDictionary<string, string> FailedAssemblies = new ConcurrentDictionary<string, string>();

        public Dictionary<string, Assembly> AppdomainAssembliesByName
        {
            get;
            private set;
        } = new Dictionary<string, Assembly>();


        public HashSet<string> DynamicAppdomainAssemblies
        {
            get;
        } = new HashSet<string>();


        public bool IsDefaultLoaded
        {
            get;
            set;
        }

        public static LoadIntellisenseAssembliesContext Create()
        {
            LoadIntellisenseAssembliesContext loadIntellisenseAssembliesContext = new LoadIntellisenseAssembliesContext();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            loadIntellisenseAssembliesContext.AppdomainAssembliesByName = (from a in assemblies
                                                                           group a by a.GetCachedShortName() into g
                                                                           select g.First()).ToDictionary((Assembly a) => a.GetCachedShortName());
            loadIntellisenseAssembliesContext.DynamicAppdomainAssemblies.AddRange(from a in assemblies
                                                                                  where a.IsDynamic
                                                                                  select a.FullName);
            return loadIntellisenseAssembliesContext;
        }
    }
}
