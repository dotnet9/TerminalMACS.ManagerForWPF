using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Interfaces.Service
{
    public interface IServiceLocator : IDisposable
    {
        void RegisterTypeSingleton<TService, TServiceImplementation>() where TServiceImplementation : TService;
        void RegisterTypeSingleton<TServiceImplementation>();

        void RegisterType<TService, TServiceImplementation>() where TServiceImplementation : TService;
        void RegisterType<TServiceImplementation>();

        void RegisterInstance<TService>(TService instance) where TService : class;

        TService ResolveType<TService>();
        TService TryResolveType<TService>();

        IServiceLocator CreateNew(Predicate<IServiceLocator> registry);
    }
}
