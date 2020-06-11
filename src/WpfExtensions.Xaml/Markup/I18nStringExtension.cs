using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfExtensions.Xaml.Markup
{

    [ContentProperty(nameof(Args))]
    [MarkupExtensionReturnType(typeof(object))]
    // ReSharper disable once InconsistentNaming
    public partial class I18nStringExtension : MultiBinding
    {
        private int _keyIndex;
        private ComponentResourceKey _key;

        public ComponentResourceKey Key
        {
            get => _key;
            set
            {
                _keyIndex = Bindings.Count;
                _key = value;
            }
        }

        public ArgCollection Args { get; }

        [ConstructorArgument(nameof(ResourceConverter))]
        public IValueConverter ResourceConverter { get; set; }

        public new IMultiValueConverter Converter
        {
            get => base.Converter;
            private set => base.Converter = value;
        }

        private class MultiValueConverter : IMultiValueConverter
        {
            private readonly I18nStringExtension _owner;

            public MultiValueConverter(I18nStringExtension owner) => _owner = owner;

            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                var keyValue = values[_owner._keyIndex];
                if (keyValue is string @string)
                {
                    return (values.Length + _owner.Args.Count) > 1
                        ? string.Format(@string, _owner.Args.Indexes.Select(item => item.InBindings
                            ? values[item.Index]
                            : _owner.Args[item.Index]).ToArray())
                        : values.Single();
                }

                throw new ArgumentException($"The {nameof(I18nStringExtension)} does not support {keyValue?.GetType()} type, " +
                                            $"please try to use {nameof(I18nExtension)}");
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException();
            }
        }
    }

    public class ArgCollection : Collection<object>
    {
        // HACK: Replace the System.ValueTuple with a struct.
        // See here for details: https://github.com/dotnet/wpf/issues/2320
        internal struct ArgTuple
        {
            public bool InBindings { get; }

            public int Index { get; }

            public ArgTuple(bool inBindings, int index)
            {
                InBindings = inBindings;
                Index = index;
            }
        }

        private readonly I18nStringExtension _owner;

        internal List<ArgTuple> Indexes = new List<ArgTuple>();

        public ArgCollection(I18nStringExtension owner) => _owner = owner;

        protected override void InsertItem(int index, object item)
        {
            if (item is BindingBase binding)
            {
                Indexes.Add(new ArgTuple(true, _owner.Bindings.Count));
                _owner.Bindings.Add(binding);
            }
            else
            {
                Indexes.Add(new ArgTuple(false, Count));
                base.InsertItem(index, item);
            }
        }
    }
}
