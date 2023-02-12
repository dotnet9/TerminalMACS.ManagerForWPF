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
    /// 当值为true时是可见，false为折叠
    /// </summary>
    public class BooleanToCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility show = (Visibility)value;
            if (show == Visibility.Collapsed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
