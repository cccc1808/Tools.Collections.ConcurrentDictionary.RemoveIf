using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.StructContainers;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model
{
    public static class ConcurrentDictionaryExtensions
    {
        #region

        public static bool RemoveIf<TKey, TData>(
            this ConcurrentDictionary<TKey, StructItemWrapper<TData>> dictionary,
            TKey key,
            Func<StructItemWrapper<TData>, bool> removeIfFunc
            )
        {
            var collection = (ICollection<KeyValuePair<TKey, StructItemWrapper<TData>>>)dictionary;
            return RemoveIf(
                collection,
                key,
                removeIfFunc
                );
        }

        public static void RemoveIf<TKey, TData>(
            this ConcurrentDictionary<TKey, IValueWrapper<TData>> dictionary,
            TKey key,
            Func<IValueWrapper<TData>, bool> removeIfFunc,
            out RemoveResult<TData> result
            )
        {
            var collection = (ICollection<KeyValuePair<TKey, IValueWrapper<TData>>>)dictionary;
            RemoveIf(
                collection,
                key,
                removeIfFunc,
                out result
                );
        }

        public static void RemoveIf<TKey, TData, TParam>(
            this ConcurrentDictionary<TKey, IValueWrapper<TData>> dictionary,
            TKey key,
            Func<IValueWrapper<TData>, TParam, bool> removeIfFunc,
            TParam param,
            out RemoveResult<TData> result
            )
        {
            var collection = (ICollection<KeyValuePair<TKey, IValueWrapper<TData>>>)dictionary;
            RemoveIf(
                collection,
                key,
                removeIfFunc,
                param,
                out result
                );
        }       

        #endregion


        #region

        public static bool RemoveIf<TKey, TData>(
            this ICollection<KeyValuePair<TKey, StructItemWrapper<TData>>> collection,
            TKey key,
            Func<StructItemWrapper<TData>, bool> removeIfFunc
            )
        {
            var removeItem = new StructItemWrapper<TData>(removeIfFunc);

            var isRemoved = collection
                .Remove(
                    new KeyValuePair<TKey, StructItemWrapper<TData>>(
                        key,
                        removeItem
                        )
                    );

            return isRemoved;
        }

        public static void RemoveIf<TKey, TData>(
            this ICollection<KeyValuePair<TKey, IValueWrapper<TData>>> collection,
            TKey key,
            Func<IValueWrapper<TData>, bool> removeIfFunc,
            out RemoveResult<TData> result
            )
        {
            var removeExpressionItem = new RemoveFuncItem<TData>(removeIfFunc);

            var isRemoved = collection
                .Remove(
                    new KeyValuePair<TKey, IValueWrapper<TData>>(
                        key,
                        removeExpressionItem
                        )
                    );

            result = new RemoveResult<TData>(
                isRemoved: isRemoved,
                data: removeExpressionItem.CaptureEqualsItem
                );
        }

        public static void RemoveIf<TKey, TData, TParam>(
            this ICollection<KeyValuePair<TKey, IValueWrapper<TData>>> collection,
            TKey key,
            Func<IValueWrapper<TData>, TParam, bool> removeIfFunc,
            TParam param,
            out RemoveResult<TData> result
            )
        {
            var removeExpressionItem = new RemoveFuncItemWithParam<TData, TParam>(removeIfFunc, param);

            var isRemoved = collection
                .Remove(
                    new KeyValuePair<TKey, IValueWrapper<TData>>(
                        key,
                        removeExpressionItem
                        )
                    );

            result = new RemoveResult<TData>(
                isRemoved: isRemoved,
                data: removeExpressionItem.CaptureEqualsItem
                );
        }

        #endregion
    }
}