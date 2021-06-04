using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using MvvmCrossDemo.Core.Services;

namespace MvvmCrossDemo.Core.ViewModels
{
	public class TipViewModel : MvxViewModel
	{
		private readonly ICalculationService _calculationService;

		public TipViewModel(ICalculationService calculationService)
		{
			_calculationService = calculationService;
		}

		public override async Task Initialize()
		{
			await base.Initialize();

			_subTotal = 100;
			_generosity = 10;

			Recalculate();
		}

		private double _subTotal;
		public double SubTotal
		{
			get => _subTotal;
			set
			{
				_subTotal = value;
				RaisePropertyChanged(() => SubTotal);

				Recalculate();
			}
		}
		
		private int _generosity;
		public int Generosity
		{
			get => _generosity;
			set
			{
				_generosity = value;
				RaisePropertyChanged(() => Generosity);

				Recalculate();
			}
		}


		private double _tip;
		public double Tip
		{
			get => _tip;
			set
			{
				_tip = value;
				RaisePropertyChanged(() => Tip);
			}
		}

		private void Recalculate()
		{
			Tip = _calculationService.TipAmount(SubTotal, Generosity);
		}
	}
}
