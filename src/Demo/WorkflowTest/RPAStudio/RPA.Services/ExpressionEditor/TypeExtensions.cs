using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public static class TypeExtensions
    {
        public static string FriendlyName(this Type type, bool fullName = false, bool omitGenericArguments = false)
        {
            string text = fullName ? type.FullName : type.Name;
            if (type.IsGenericType)
            {
                int num = text.IndexOf('`');
                if (num > 0)
                {
                    text = text.Substring(0, num);
                }
                if (omitGenericArguments)
                {
                    return text;
                }
                Type[] genericArguments = type.GetGenericArguments();
                StringBuilder stringBuilder = new StringBuilder(text);
                stringBuilder.Append("<");
                for (int i = 0; i < genericArguments.Length - 1; i++)
                {
                    stringBuilder.AppendFormat("{0},", FriendlyName(genericArguments[i]), fullName);
                }
                stringBuilder.AppendFormat("{0}>", FriendlyName(genericArguments[genericArguments.Length - 1]), fullName);
                return stringBuilder.ToString();
            }
            return text;
        }
    }

}
