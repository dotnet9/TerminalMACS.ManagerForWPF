using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfExtensions.Xaml.Markup
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    [ContentProperty(nameof(Converters))]
    public partial class ComposeExtension : MarkupExtension, IValueConverter
    {
        [ConstructorArgument(nameof(Converters))]
        public ConverterCollection Converters { get; } = new ConverterCollection();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            Converters.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            Converters.Aggregate(value, (current, converter) => converter.ConvertBack(current, targetType, parameter, culture));
    }

    public class ConverterCollection : Collection<IValueConverter>
    {
        public void Add(object item)
        {
            if (item is IValueConverter converter)
            {
                base.Add(converter);
            }
            else
            {
                throw new ArgumentException($"[ComposeExtension] The type of the parameter must be IValueConverter, " +
                    $"but here is {item?.GetType().FullName ?? "null"}");
            }
        }
    }
}
