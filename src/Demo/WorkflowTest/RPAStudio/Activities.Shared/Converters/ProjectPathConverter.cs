using RPA.Interfaces.Share;
using RPA.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Activities.Shared.Converters
{
    public class ProjectPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var filePath = (string)value;

            var retPath = "";
            if (!System.IO.Path.IsPathRooted(filePath))
            {
                //是相对路径，则转成绝对路径以便显示
                if (parameter == null)
                {
                    retPath = System.IO.Path.Combine(SharedObject.Instance.ProjectPath, filePath);
                }
                else
                {
                    retPath = System.IO.Path.Combine(SharedObject.Instance.ProjectPath + @"\" + (string)parameter, filePath);
                }

            }
            else
            {
                //是绝对路径，不做转换
                retPath = filePath;
            }

            //不直接返回路径而是返回ImageSource是为了避免图片被一直引用而无法删除（删除未使用的截图）
            if (System.IO.File.Exists(retPath))
            {
                return Common.BitmapFromUri(new Uri(retPath));
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
