using Microsoft.VisualBasic.Activities;
using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Activities.Shared.Converters
{
    public class VariableToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ModelItem modelItem = value as ModelItem;
            if (value == null)
            {
                return null;
            }
            InArgument inArgument = (modelItem == null) ? (value as InArgument) : (modelItem.GetCurrentValue() as InArgument);
            if (inArgument != null)
            {
                if (inArgument.Expression == null)
                {
                    return inArgument.ToString();
                }
                dynamic expression = inArgument.Expression;
                string text;
                try
                {
                    if (expression.ExpressionText == null)
                    {
                        return null;
                    }
                    text = expression.ExpressionText.ToString();
                    if (string.IsNullOrEmpty(text))
                    {
                        text = null;
                    }
                }
                catch
                {
                    text = inArgument.Expression.ToString();
                }
                if (parameter != null)
                {
                    text = string.Format("{0} 是 {1}", parameter.ToString(), text);
                }
                return text;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                InArgument<string> inArgument = new InArgument<string>();
                if (value == null)
                {
                    inArgument.Expression = null;
                }
                string text = value?.ToString();
                if (string.IsNullOrWhiteSpace(text))
                {
                    inArgument.Expression = null;
                }
                inArgument.Expression = new VisualBasicValue<string>(text);
                return inArgument;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
