using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WpfExtensions.Xaml.Converters
{
    #region Without parameter

    public class AnyOperator : ValueConverterBase<IEnumerable, bool>
    {
        protected override bool Convert(IEnumerable value)
        {
            if (value == null) return false;

            var enumerator = value.GetEnumerator();
            var result = enumerator.MoveNext();
            if (enumerator is IDisposable disposable) disposable.Dispose();
            return result;
        }
    }

    public class NotOperator : ValueConverterBase<bool, bool>
    {
        protected override bool Convert(bool value) => !value;

        protected override bool ConvertBack(bool value) => !value;
    }

    public class IsNullOperator : ValueConverterBase<object, bool>
    {
        protected override bool Convert(object value) => Equals(value, null);
    }

    public class IsNullOrEmptyOperator : ValueConverterBase<string, bool>
    {
        protected override bool Convert(string value) => string.IsNullOrEmpty(value);
    }

    public class ReverseOperator : ValueConverterBase<IList, IList>
    {
        protected override IList ConvertNonNullValue(IList value) => Reverse(value).ToList();

        private static IEnumerable<object> Reverse(IList list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                yield return list[i];
            }
        }
    }

    public class GetTypeOperator : ValueConverterBase<object, Type>
    {
        protected override Type ConvertNonNullValue(object value) => value.GetType();
    }

    #endregion

    #region With parameter

    public class DoubleEqualsOperator : ValueConverterBase<double, bool, double>
    {
        private const double Epsilon = 1E-6;

        protected override bool Convert(double value, double parameter) => Math.Abs(value - parameter) < Epsilon;
    }

    public class EqualsOperator : ValueConverterBase<object, bool, object>
    {
        protected override bool Convert(object value, object parameter) => value?.Equals(parameter) ?? parameter == null;
    }

    public class GreaterThanOperator : ValueConverterBase<double, bool, double>
    {
        protected override bool Convert(double value, double parameter) => value > parameter;
    }

    public class LessThanOperator : ValueConverterBase<double, bool, double>
    {
        protected override bool Convert(double value, double parameter) => value < parameter;
    }

    #endregion
}
