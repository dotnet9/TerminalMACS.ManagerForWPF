using ActiproSoftware.Windows;
using ActiproSoftware.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RPA.Shared.Converters
{
    public class ResourceKeyToColorConverter : IValueConverter
    {
        private const string ThemeResourcesLocation = "Themes/Includes/Assets/";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            try
            {
                ResourceKey resourceKey = value as ResourceKey;
                if (resourceKey == null)
                {
                    result = value;
                }
                else
                {
                    Application application = Application.Current;
                    SolidColorBrush solidColorBrush = ((application != null) ? application.TryFindResource(resourceKey) : null) as SolidColorBrush;
                    if (solidColorBrush != null)
                    {
                        result = solidColorBrush.Color;
                    }
                    else
                    {
                        ResourceDictionary currentThemeResources = this.GetCurrentThemeResources();
                        SolidColorBrush solidColorBrush2 = currentThemeResources[resourceKey] as SolidColorBrush;
                        if (solidColorBrush2 == null && currentThemeResources.MergedDictionaries != null && currentThemeResources.MergedDictionaries.Count > 0)
                        {
                            foreach (ResourceDictionary resourceDictionary in currentThemeResources.MergedDictionaries)
                            {
                                solidColorBrush2 = (resourceDictionary[resourceKey] as SolidColorBrush);
                                if (solidColorBrush2 != null)
                                {
                                    return solidColorBrush2.Color;
                                }
                            }
                        }
                        result = ((solidColorBrush2 != null) ? solidColorBrush2.Color : value);
                    }
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private ResourceDictionary GetCurrentThemeResources()
        {
            string text = ThemeManager.CurrentTheme;
            if (string.IsNullOrWhiteSpace(text))
            {
                if (text.StartsWith(ThemeName.MetroLight.ToString()))
                {
                    text = ThemeName.MetroLight.ToString();
                }
                else if (text.StartsWith(ThemeName.MetroWhite.ToString()))
                {
                    text = ThemeName.MetroWhite.ToString();
                }
                else
                {
                    text = ThemeName.MetroLight.ToString();
                }
            }
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            try
            {
                Assembly assembly = typeof(SharedThemeCatalog).Assembly;
                resourceDictionary.Source = ResourceHelper.GetLocationUri(assembly, string.Format("{0}{1}.xaml", "Themes/Includes/Assets/", text));
            }
            catch
            {

            }

            return resourceDictionary;
        }
    }
}
