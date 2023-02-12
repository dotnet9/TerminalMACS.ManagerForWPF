using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RPA.Shared.Converters
{
    /// <summary>
    /// 当前为true时隐藏，为false时显示
    /// </summary>
    public class OppositeBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility show = (Visibility)value;
            if (show == Visibility.Collapsed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
