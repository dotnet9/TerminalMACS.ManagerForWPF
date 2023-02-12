using Autofac;
using RPA.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.Service
{
    public class AutofacServiceLocator : IServiceLocator
    {
        private bool _isDisposed;
        private ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ILifetimeScope _scope;

        public AutofacServiceLocator()
        {
            _containerBuilder = new ContainerBuilder();

            RegisterSelfServices();
        }

        private AutofacServiceLocator(ContainerBuilder builder)
        {
            this._containerBuilder = builder;
        }

        private IContainer WithContainer()
        {
            if (_container == null)
            {
                _container = _containerBuilder.Build();
            }

            return _container;
        }

        private ILifetimeScope WithScope()
        {
            if (_scope == null)
            {
                _scope = WithContainer().BeginLifetimeScope();
            }

            return _scope;
        }

        private void RegisterSelfServices()
        {
            _containerBuilder.RegisterInstance<IServiceLocator>(this);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            _containerBuilder.RegisterInstance(instance).As<TService>();
        }

        public void RegisterType<TService, TServiceImplementation>() where TServiceImplementation : TService
        {
            _containerBuilder.RegisterType<TServiceImplementation>().As<TService>().ExternallyOwned();
        }

        public void RegisterType<TServiceImplementation>()
        {
            _containerBuilder.RegisterType<TServiceImplementation>().ExternallyOwned();
        }

        public void RegisterTypeSingleton<TService, TServiceImplementation>() where TServiceImplementation : TService
        {
            _containerBuilder.RegisterType<TServiceImplementation>().As<TService>().SingleInstance();
        }

        public void RegisterTypeSingleton<TServiceImplementation>()
        {
            _containerBuilder.RegisterType<TServiceImplementation>().SingleInstance();
        }

        public TService ResolveType<TService>()
        {
            return WithScope().Resolve<TService>();
        }

        public TService TryResolveType<TService>()
        {
            TService result;
            WithScope().TryResolve(out result);
            return result;
        }

        public IServiceLocator CreateNew(Predicate<IServiceLocator> registry)
        {
            AutofacServiceLocator locator = null;
            bool successful = false;
            ILifetimeScope scope = (WithContainer() as ILifetimeScope).BeginLifetimeScope(delegate (ContainerBuilder b)
            {
                locator = new AutofacServiceLocator(b);
                successful = registry(locator);
                locator.RegisterSelfServices();
            });

            locator._scope = scope;
            if (!successful)
            {
                return null;
            }

            return locator;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            IContainer container = _container;
            if (container != null)
            {
                container.Dispose();
            }

            ILifetimeScope scope = _scope;
            if (scope == null)
            {
                return;
            }
            scope.Dispose();
        }
    }
}
