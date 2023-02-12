using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RPA.Shared.Converters
{
    /// <summary>
    /// 当为true时为折叠，false为可见
    /// </summary>
    public class OppositeBooleanToCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
