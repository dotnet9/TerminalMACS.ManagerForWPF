using GalaSoft.MvvmLight.Threading;
using NLog;
using RPA.Interfaces.Service;
using RPA.Services.App;
using RPA.Shared.Configs;
using RPAStudio.ViewModel;
using RPAStudio.Views;
using System;
using System.Windows;

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
