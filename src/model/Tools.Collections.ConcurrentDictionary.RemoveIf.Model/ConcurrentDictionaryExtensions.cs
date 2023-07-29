using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model
{
    public static class ConcurrentDictionaryExtensions
    {
        /// <typeparam name="TKey">Item Key</typeparam>
        /// <typeparam name="TData">Data Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="removeIfFunc">Remove if true</param>
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

        /// <typeparam name="TKey">Item Key</typeparam>
        /// <typeparam name="TData">Data Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="removeIfFunc">Remove if true</param>
        public static void RemoveIf<TKey, TData>(
            this ICollection<KeyValuePair<TKey, IValueWrapper<TData>>> collection,
            TKey key,
            Func<IValueWrapper<TData>, bool> removeIfFunc,
            out RemoveResult<TData> result
            )
        {
            var removeExpressionItem = new RemoveExpressionItem<TData>(removeIfFunc);

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
    }
}