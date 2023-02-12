using RPA.Interfaces.App;
using RPA.Services.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RPA.Interfaces.Service;
using RPAStudio.Views;
using NLog;
using GalaSoft.MvvmLight.Threading;
using RPAStudio.ViewModel;
using RPA.Shared.Configs;

namespace RPAStudio.App
{
    public class RPAStudioApplication : StudioApplication
    {
        private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

        private IServiceLocator _serviceLocator;

        public RPAStudioApplication(IServiceLocator serviceLocator) : base(serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

       
        public override void OnStart()
        {
            _logger.Debug("程序启动");

            //MVVM帮助类初始化，初始化后才能正常使用Messenger类
            DispatcherHelper.Initialize();

            AppPathConfig.InitConfigs();
        }

        public override void OnShutdown()
        {
            _logger.Debug("程序结束");
        }

        public override Uri OnGetAppXaml()
        {
            return new Uri("pack://application:,,,/Resources/App.Resources.xaml");
        }

        public override Window OpenMainWindow()
        {
            return new MainWindow();
        }

        public override void OnException()
        {
            _serviceLocator.ResolveType<MainViewModel>().IsLoading = false;
        }

       
    }
}
