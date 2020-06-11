// ReSharper disable once CheckNamespace

using System.Collections.Generic;

namespace WpfExtensions.Xaml.ExtensionMethods
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
