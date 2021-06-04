using MvvmCross;
using MvvmCross.ViewModels;
using MvvmCrossDemo.Core.Services;
using MvvmCrossDemo.Core.ViewModels;

namespace MvvmCrossDemo.Core
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			Mvx.IoCProvider.RegisterType<ICalculationService, CalculationService>();

			RegisterAppStart<TipViewModel>();
		}
	}
}
