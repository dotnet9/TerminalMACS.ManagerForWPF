using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TerminalMACS.Utils.NetObjectHelper
{
    /// <summary> 
    /// Represents a dynamic data collection that provides notifications when items get added, removed, or when the whole list is refreshed. 
    /// </summary> 
    /// <typeparam name="T"></typeparam> 
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        /// <summary> 
        /// Adds the elements of the specified collection to the end of the ObservableRangeCollection(Of T). 
        /// </summary> 
        public void AddRange(IEnumerable<T> collection, bool needUpdate = true)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            foreach (var i in collection) Items.Add(i);
            if (needUpdate)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary> 
        /// Removes the first occurence of each item in the specified collection from ObservableRangeCollection(Of T). 
        /// </summary> 
        public void RemoveRange(IEnumerable<T> collection, bool needUpdate = true)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            foreach (var i in collection) Items.Remove(i);
            if (needUpdate)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void UpdateRowIndex()
        {
            try
            {
                var type = typeof(T);
                var pr = type.GetProperty("RowIndex");
                foreach (var item in Items)
                {
                    pr.SetValue(item, (Items.IndexOf(item) + 1), null);
                }
                this.UpdateSource();
            }
            catch { }
        }
        /// <summary> 
        /// Clears the current collection and replaces it with the specified item. 
        /// </summary> 
        public void Replace(T item)
        {
            ReplaceRange(new T[] { item });
        }

        /// <summary> 
        /// Clears the current collection and replaces it with the specified collection. 
        /// </summary> 
        public void ReplaceRange(IEnumerable<T> collection, bool needUpdate = true)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            Items.Clear();
            foreach (var i in collection) Items.Add(i);
            if (needUpdate)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        //
        public void RemoveAt(int index, bool needUpdate)
        {
            Items.RemoveAt(index);
            if (needUpdate)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void UpdateSource()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear(bool needUpdate)
        {
            Items.Clear();
            if (needUpdate)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary> 
        /// Initializes a new instance of the System.Collections.ObjectModel.ObservableRangeCollection(Of T) class. 
        /// </summary> 
        public ObservableRangeCollection()
            : base() { }

        /// <summary> 
        /// Initializes a new instance of the System.Collections.ObjectModel.ObservableRangeCollection(Of T) class that contains elements copied from the specified collection. 
        /// </summary> 
        /// <param name="collection">collection: The collection from which the elements are copied.</param> 
        /// <exception cref="System.ArgumentNullException">The collection parameter cannot be null.</exception> 
        public ObservableRangeCollection(IEnumerable<T> collection)
            : base(collection) { }
    }


    /// <summary>
    /// 数据列表扩展
    /// </summary>
    public static class ObservableCollectionExtension
    {
        ///// <summary>
        ///// 批量添加数据
        ///// </summary>
        ///// <param name="rangeData"></param>
        //public static void AddRange<T>(this ObservableRangeCollection<T> lst, IEnumerable<T> collection)
        //{
        //    foreach (var item in collection)
        //    {
        //        lst.Add(item);
        //    }
        //}

        ///// <summary>
        ///// 批量替换
        ///// </summary>
        ///// <param name="collection"></param>
        //public static void ReplaceRange<T>(this ObservableRangeCollection<T> lst, IEnumerable<T> collection)
        //{
        //    lst.Clear();
        //    AddRange(lst, collection); 
        //}

        //
        // 摘要:
        //     根据键按升序或者降序对序列的元素排序。
        //
        // 参数:
        //   source:
        //     一个要排序的值序列。
        //
        //   keySelector:
        //     用于从元素中提取键的函数。
        //
        //   orderyAsc:
        //     排序方向，true:升序排序，false:降序排序，默认为true
        //
        // 类型参数:
        //   TSource:
        //     source 中的元素的类型。
        //
        //   TKey:
        //     keySelector 返回的键的类型。
        //
        // 返回结果:
        //     一个 System.Linq.IOrderedEnumerable`1，其元素按键排序。
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     source 或 keySelector 为 null。
        public static ObservableRangeCollection<TSource> OrderByEx<TSource, TKey>(this ObservableRangeCollection<TSource> source
            , Func<TSource, TKey> keySelector, bool orderyAsc = true, bool needUpdate = true)
        {
            List<TSource> sortedLst = null;
            if (orderyAsc)
                sortedLst = source.OrderBy(keySelector).ToList();
            else
                sortedLst = source.OrderByDescending(keySelector).ToList();

            source.ReplaceRange(sortedLst, needUpdate);

            return source;
        }
    }
}
