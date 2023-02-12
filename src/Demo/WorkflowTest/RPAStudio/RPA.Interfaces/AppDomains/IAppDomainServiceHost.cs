using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.AppDomains
{
    public interface IAppDomainServiceHost
    {
        void Init();

        TService GetService<TService>();

        void RegisterCrossDomainInstance<TService>(TService instance) where TService : class;

        void RegisterServices();
        
    }
}
