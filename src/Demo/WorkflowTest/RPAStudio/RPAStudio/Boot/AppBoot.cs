using NLog;
using RPA.Interfaces.App;
using RPA.Shared.Utils;
using RPAStudio.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RPAStudio.Boot
{
    class AppBoot
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();
        public static AppServiceRegistry AppServiceRegistry = new RPAStudioServiceRegistry();
        private static IApplication _app;

        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            AppServiceRegistry.RegisterServices();
            _app = AppServiceRegistry.ResolveType<IApplication>();

            _app.Start(args);
            _app.Shutdown(); 
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Common.InvokeAsyncOnUI(() =>
            {
                _app.OnException();

                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    _logger.Error(exception,"触发异常");
                    MessageBox.Show("程序运行过程中出现了异常，请联系软件开发商！");
                }
            });
        }
    }
}
