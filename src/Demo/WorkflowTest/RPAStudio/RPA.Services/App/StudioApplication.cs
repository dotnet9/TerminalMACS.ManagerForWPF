using NLog;
using RPA.Interfaces.App;
using RPA.Interfaces.Service;
using RPA.Shared.Localization;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RPA.Services.App
{
    public abstract class StudioApplication : IApplication
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;
        private Application _app;

        public StudioApplication(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        private Application InitApp()
        {
            //ActiproLicenseManager.RegisterLicense(授权用户名, 授权用户密钥);
            ActiproLocalization.Init();

            var app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            app.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            var appXaml = OnGetAppXaml();
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = appXaml
            };

            app.Resources.MergedDictionaries.Add(resourceDictionary);

            return app;
        }


        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            _logger.Error("默认域UI线程触发异常:" + e.Exception.ToString());

            CommonMessageBox.ShowError("程序触发内部异常，请检查日志！");
        }

        public void Start(string[] args)
        {
#if DEBUG
            Common.OpenConsole();
#endif
            _app = InitApp();

            OnStart();

            var window = OpenMainWindow();
            window?.ShowDialog();
        }

        public void Shutdown()
        {
            OnShutdown();

#if DEBUG
            Common.CloseConsole();
#endif
        }

        public abstract void OnStart();

        public abstract void OnShutdown();

        public abstract Uri OnGetAppXaml();

        public abstract Window OpenMainWindow();

        public abstract void OnException();
    }
}
