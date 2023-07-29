using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.StructContainers;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Test.UnitTest.LinkCounterTest
{
    [TestClass]
    public class LinkCounterStructTest
    {
        [TestMethod]
        public void Test()
        {
            ConcurrentDictionary<
                string,
                StructItemWrapper<LinkItemStruct<string>>
                >
                dictionary
                    = new ConcurrentDictionary<
                        string,
                        StructItemWrapper<LinkItemStruct<string>>
                        >();

            var key = "key";            

            {
                var valueFactory = (string k) => "value";
                var updateValueFunc = (string k, string e) => e;

                //Add first link to key
                AddLink(dictionary, key, valueFactory, updateValueFunc);
                //Add second link to key
                AddLink(dictionary, key, valueFactory, updateValueFunc);

                var item = dictionary[key];

                Assert.AreEqual(1, dictionary.Count);
                Assert.AreEqual(2, item.Value.LinkCount);
            }

            {
                //Remove first link to key
                RemoveLink(dictionary, key, out var removeResult1);
                //Remove second link to key and remove key Item
                RemoveLink(dictionary, key, out var removeResult2);

                Assert.IsFalse(removeResult1.IsRemoved);
                Assert.AreEqual(1, removeResult1.Data.LinkCount);

                Assert.IsTrue(removeResult2.IsRemoved);
                Assert.AreEqual(0, removeResult2.Data.LinkCount);
            }

        }


        #region

        public static StructItemWrapper<LinkItemStruct<TValue>> AddLink<TKey, TValue>(
            ConcurrentDictionary<
                TKey,
                StructItemWrapper<LinkItemStruct<TValue>>
                >
            dictionary,
            TKey key,
            Func<TKey, TValue> valueFactorFunc,
            Func<TKey, TValue, TValue> updateValueFunc
            )
            where TKey : notnull
        {
            return dictionary
                .AddOrUpdate(
                    key: key,
                    addValueFactory: (k) => LinkItemStruct<TValue>.CreateOneLinkWrapper(valueFactorFunc(k)),
                    updateValueFactory: (k, e) => e.Value.IncrementCounterWrapper(updateValueFunc(k, e.Value.Value))
                    );
        }

        public static void RemoveLink<TKey, TValue>(                    
            ConcurrentDictionary<                    
                TKey,
                StructItemWrapper<LinkItemStruct<TValue>>                    
                > 
            dictionary,
            TKey key,
            out RemoveResult<LinkItemStruct<TValue>> result
            )
            where TKey : notnull
        {
            var changeCounterResult = dictionary
                .AddOrUpdate(
                    key: key,
                    addValueFactory: static (k) => LinkItemStruct<TValue>.CreateZeroLinkWrapper(),
                    updateValueFactory: static (k,e) => e.Value.DecrementCounterWrapper()
                    );

            //Remove item if link count = 0
            //if (result.Value.LinkCount != 0)
            //{
            //    return false;
            //}

            LinkItemStruct<TValue> removeItem = default;
            var isRemoved = dictionary.RemoveIf(
                key: key,
                removeIfFunc: e =>
                {
                    removeItem = e.Value;
                    return e.Value.LinkCount == 0;
                }
                );

            result = new RemoveResult<LinkItemStruct<TValue>>(isRemoved, removeItem);
        }

        #endregion
    }
}