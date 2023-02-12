using RPA.Interfaces.AppDomains;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPA.Interfaces.AppDomains
{
    public abstract class MarshalByRefServiceProxyBase<TService> : MarshalByRefServiceBase where TService : class
    {
        private IAppDomainControllerService _appDomainControllerService;

        protected TService _service;

        protected TService InnerService
        {
            get
            {
                return _service;
            }
            set
            {
                _service = value;
            }
        }


        protected MarshalByRefServiceProxyBase(IAppDomainControllerService appDomainControllerService)
        {
            _appDomainControllerService = appDomainControllerService;

            _appDomainControllerService.ChildAppDomainCreated += _appDomainControllerService_ChildAppDomainCreated;
            _appDomainControllerService.ChildAppDomainUnloading += _appDomainControllerService_ChildAppDomainUnloading;

            ConnectToInnerService();
        }


        private void _appDomainControllerService_ChildAppDomainCreated(object sender, EventArgs e)
        {
            Common.InvokeOnUI(() =>
            {
                ConnectToInnerService();
                OnAfterConnectToInnerService();
            });
        }

        private void _appDomainControllerService_ChildAppDomainUnloading(object sender, EventArgs e)
        {
            Common.InvokeOnUI(() =>
            {
                OnBeforeDisconnectFromInnerService();
                DisconnectFromInnerService();
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisconnectFromInnerService();
                _appDomainControllerService.ChildAppDomainCreated -= _appDomainControllerService_ChildAppDomainCreated;
                _appDomainControllerService.ChildAppDomainUnloading -= _appDomainControllerService_ChildAppDomainUnloading;
            }

            base.Dispose(disposing);
        }


        private void ConnectToInnerService()
        {
            if (InnerService != null)
            {
                return;
            }

            InnerService = _appDomainControllerService.GetHostService<TService>();
            OnAfterConnectToInnerService();
        }

        protected virtual void OnAfterConnectToInnerService()
        {
        }

        protected virtual void OnBeforeDisconnectFromInnerService()
        {
        }

        private void DisconnectFromInnerService()
        {
            if (InnerService == null)
            {
                return;
            }

            _service = default(TService);
        }

    }
}
