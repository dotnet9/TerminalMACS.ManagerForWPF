using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LazyLoadWebImage
{
	public class PaginationTestViewModel : BindableBase
	{
		private ICommand showCommand;
		/// <summary>
		///     页码改变命令
		/// </summary>
		public ICommand ShowCommand => (showCommand ?? new DelegateCommand(async () => await RaiseShowCommand()));

		private ObservableCollection<PageSizeInfo>? pageSizeItemSource;
		public ObservableCollection<PageSizeInfo> PageSizeItemSource => (pageSizeItemSource ?? new ObservableCollection<PageSizeInfo>
		{
				new PageSizeInfo( 60,"60 Item / Page"),
				new PageSizeInfo( 120,"120 Item / Page"),
				new PageSizeInfo( 180,"180 Item / Page"),
				new PageSizeInfo( 240,"240 Item / Page"),
		});
		private int _pageSize = 60;
		public int PageSize
		{
			get { return _pageSize; }
			set { SetProperty(ref _pageSize, value); }
		}
		private string _PageCountString = "共100项";
		public string PageCountString
		{
			get { return _PageCountString; }
			set { SetProperty(ref _PageCountString, value); }
		}
		private int _PageIndex = 1;
		public int PageIndex
		{
			get { return _PageIndex; }
			set { SetProperty(ref _PageIndex, value); }
		}
		private int _PageCount = 101;
		public int PageCount
		{
			get { return _PageCount; }
			set { SetProperty(ref _PageCount, value); }
		}


		/// <summary>
		///     页码改变
		/// </summary>
		private async Task RaiseShowCommand()
		{
			MessageBox.Show($"{PageSizeItemSource.Count} + {PageSize}");
		}
	}
}
