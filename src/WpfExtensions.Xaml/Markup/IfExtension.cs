using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using WpfExtensions.Xaml.ExtensionMethods;

namespace WpfExtensions.Xaml.Markup;

public class IfExtension : MultiBinding
{
    private const int InvalidIndex = -1;

    private int _conditionIndex = InvalidIndex;

    private int _count;
    private object _false;
    private int _falseIndex = InvalidIndex;

    private object _true;
    private int _trueIndex = InvalidIndex;

    public IfExtension()
    {
        Converter = new MultiValueConverter(this);
    }

    public IfExtension(Binding condition, object trueValue, object falseValue)
        : this()
    {
        Condition = condition;
        True = trueValue;
        False = falseValue;
    }

    [ConstructorArgument(nameof(Condition))]
    public Binding Condition
    {
        set => SetProperty(value, ref _conditionIndex, out _);
    }

    [ConstructorArgument(nameof(True))]
    public object True
    {
        set => SetProperty(value, ref _trueIndex, out _true);
    }

    [ConstructorArgument(nameof(False))]
    public object False
    {
        set => SetProperty(value, ref _falseIndex, out _false);
    }

    public new IMultiValueConverter Converter
    {
        get => base.Converter;
        private set => base.Converter = value;
    }

    private void SetProperty<T>(T value, ref int index, out T storage)
    {
        if (index != InvalidIndex)
            throw new InvalidOperationException("Cannot reset the value. ");

        if (value is BindingBase binding)
        {
            Bindings.Add(binding);
            index = _count++;
            storage = default;
        }
        else
        {
            storage = value;
        }
    }


    private class MultiValueConverter : IMultiValueConverter
    {
        private readonly IfExtension _ifExtension;

        public MultiValueConverter(IfExtension ifExtension)
        {
            _ifExtension = ifExtension;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var condition = values[_ifExtension._conditionIndex];

            if (condition == DependencyProperty.UnsetValue) return Binding.DoNothing;

            return condition.CastTo<bool>()
                ? GetValue(_ifExtension._trueIndex, _ifExtension._true)
                : GetValue(_ifExtension._falseIndex, _ifExtension._false);

            object GetValue(int index, object storage)
            {
                return index != InvalidIndex ? values[index] : storage;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}