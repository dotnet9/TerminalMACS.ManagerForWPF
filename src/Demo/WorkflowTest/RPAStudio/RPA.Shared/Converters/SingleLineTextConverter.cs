using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RPA.Shared.Converters
{
    /// <summary>
    /// 单行文本转换类
    /// </summary>
    public class SingleLineTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = (string)value;
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Replace(Environment.NewLine, " ");
                s = s.Replace("\n", " ");
            }

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
