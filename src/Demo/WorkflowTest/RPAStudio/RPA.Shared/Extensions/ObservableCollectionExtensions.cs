using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPA.Shared.Extensions
{
    public static class ObservableCollectionExtensions
    {
        //排序
        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector)
        {
            List<TSource> sortedList = source.OrderBy(keySelector).ToList();
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }

        //排序
        public static void Sort<T>(this ObservableCollection<T> collection, Comparison<T> comparison)
        {
            var sortableList = new List<T>(collection);
            sortableList.Sort(comparison);

            for (int i = 0; i < sortableList.Count; i++)
            {
                collection.Move(collection.IndexOf(sortableList[i]), i);
            }
        }

        //排序
        public static void Sort<T>(this ObservableCollection<T> collection) where T : IComparable
        {
            List<T> sortedList = collection.OrderBy(x => x).ToList();
            for (int i = 0; i < sortedList.Count(); i++)
            {
                collection.Move(collection.IndexOf(sortedList[i]), i);
            }
        }
    }
}
