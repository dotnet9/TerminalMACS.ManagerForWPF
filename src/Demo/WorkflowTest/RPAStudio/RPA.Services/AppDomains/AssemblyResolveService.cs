using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Share;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.AppDomains
{
    public class AssemblyResolveService : IAssemblyResolveService
    {
        private List<string> _assemblies = new List<string>();

        public AssemblyResolveService()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public void Init(List<string> assemblies)
        {
            _assemblies = assemblies;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = args.Name.Split(',')[0];

            var path = _assemblies.Where(item => System.IO.Path.GetFileNameWithoutExtension(item).Equals(name)).FirstOrDefault();

            if (System.IO.File.Exists(path))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(path);
                    return assembly;
                }
                catch (Exception)
                {
                    return null;
                }

            }
            else
            {
                path = Path.Combine(SharedObject.Instance.ApplicationCurrentDirectory, name + ".dll");
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(path);
                        return assembly;
                    }
                    catch (Exception)
                    {
                        return null;
                    }

                }

                Trace.WriteLine(string.Format("********************{0} 无法找到相应的程序集********************", args.Name));
            }

            return null;
        }


    }
}
