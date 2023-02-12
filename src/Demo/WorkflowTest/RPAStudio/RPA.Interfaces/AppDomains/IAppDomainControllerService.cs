using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.AppDomains
{
    public interface IAppDomainControllerService
    {
        event EventHandler ChildAppDomainCreated;
        event EventHandler ChildAppDomainUnloading;

        Task CreateAppDomain();

        void UnloadAppDomain();

        TService GetHostService<TService>();
    }
}
