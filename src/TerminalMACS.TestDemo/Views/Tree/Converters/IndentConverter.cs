using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TerminalMACS.TestDemo.Views.Tree.Converters
{
	public class IndentConverter : IValueConverter
	{
		public double Indent { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var item = value as TreeViewItem;
			if (null == item)
				return new Thickness(0);
			else
			{
				CusMenuInfo menu = item.DataContext as CusMenuInfo;
				return new Thickness((Indent - 1) * menu.Level, 5, 0, 0);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
