/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RPARobot"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using RPARobot.ServiceRegistry;

namespace RPARobot.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private AppServiceRegistry _locator;

        public ViewModelLocator()
        {
            _locator = RPARobotServiceRegistry.Instance;
        }


        public MainViewModel Main
        {
            get
            {
                return _locator.ResolveType<MainViewModel>();
            }
        }


        public AboutViewModel About
        {
            get
            {
                return _locator.ResolveType<AboutViewModel>();
            }
        }


        public StartupViewModel Startup
        {
            get
            {
                return _locator.ResolveType<StartupViewModel>();
            }
        }
    }
}