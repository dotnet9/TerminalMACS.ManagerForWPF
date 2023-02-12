using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Services.ExpressionEditor
{
    public static class CollectionExtensions
    {
        public static void ForEach<TItem>(this IEnumerable<TItem> collection, Action<TItem> action)
        {
            if (collection != null)
            {
                foreach (TItem item in collection)
                {
                    action(item);
                }
            }
        }

        public static void RemoveLast(this IList list)
        {
            if (list.Count != 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                collection.Add(item);
            }
        }

        public static void RemoveFirst(this IList list)
        {
            if (list.Count != 0)
            {
                list.RemoveAt(0);
            }
        }
    }

}
