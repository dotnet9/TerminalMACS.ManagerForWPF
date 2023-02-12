using NLog;
using RPA.Interfaces.Activities;
using RPA.Interfaces.AppDomains;
using RPA.Interfaces.Service;
using RPA.Interfaces.Workflow;
using RPA.Services.Activities;
using RPA.Services.Service;
using RPA.Services.Workflow;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPA.Services.AppDomains
{
    public class AppDomainServiceHost : MarshalByRefServiceBase, IAppDomainServiceHost
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private Application _app;
        private IServiceLocator _serviceLocator;

        public AppDomainServiceHost()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        public void Init()
        {
            _app = new Application();//域中必须创建Application,否则会导致跨程序集获取Uri失败
            _app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _app.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            _serviceLocator = new AutofacServiceLocator();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                _logger.Error("隔离域非UI线程触发异常:" + exception.ToString());
            }
        }

        
        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            _logger.Error("隔离域UI线程触发异常:" + e.Exception.ToString());

            CommonMessageBox.ShowError("程序触发内部异常，请检查日志！");
        }

        public TService GetService<TService>()
        {
            TService tservice = _serviceLocator.ResolveType<TService>();

            //判断类型TService是否满足继承特定基类
            if (!(tservice is MarshalByRefServiceBase))
            {
                throw new InvalidOperationException("待获取的服务必须继承自MarshalByRefServiceBase");
            }

            return tservice;
        }

        /// <summary>
        /// 在第二个域中注册
        /// </summary>
        public void RegisterServices()
        {
            _serviceLocator.RegisterTypeSingleton<IAssemblyResolveService, AssemblyResolveService>();
            _serviceLocator.RegisterTypeSingleton<IActivitiesDefaultAttributesService, ActivitiesDefaultAttributesService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowDesignerCollectService, WorkflowDesignerCollectService>();
            _serviceLocator.RegisterTypeSingleton<IWorkflowBreakpointsService, WorkflowBreakpointsService>();

            _serviceLocator.RegisterType<IActivitiesService, ActivitiesService>();
            _serviceLocator.RegisterType<IWorkflowDesignerService, WorkflowDesignerService>();

        }


        public void RegisterCrossDomainInstance<TService>(TService instance) where TService : class
        {
            _serviceLocator.RegisterInstance(instance);
        }

    }
}
