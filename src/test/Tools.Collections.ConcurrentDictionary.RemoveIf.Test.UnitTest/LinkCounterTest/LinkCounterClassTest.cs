using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Test.UnitTest.LinkCounterTest
{
    [TestClass]
    public class LinkCounterClassTest
    {
        [TestMethod]
        public void Test1()
        {
            ConcurrentDictionary<
                string,
                IValueWrapper<LinkItem<string>>
                >
                dictionary
                    = new ConcurrentDictionary<
                        string,
                        IValueWrapper<LinkItem<string>>
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

        [TestMethod]
        public void Test2_WithParam()
        {
            ConcurrentDictionary<
                string,
                IValueWrapper<LinkItem<string>>
                >
                dictionary
                    = new ConcurrentDictionary<
                        string,
                        IValueWrapper<LinkItem<string>>
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
                var fakeParam = ("NoClosureParam", DateTime.UtcNow);

                //Remove first link to key
                RemoveLink(dictionary, key, fakeParam, out var removeResult1);
                //Remove second link to key and remove key Item
                RemoveLink(dictionary, key, fakeParam, out var removeResult2);

                Assert.IsFalse(removeResult1.IsRemoved);
                Assert.AreEqual(1, removeResult1.Data.LinkCount);

                Assert.IsTrue(removeResult2.IsRemoved);
                Assert.AreEqual(0, removeResult2.Data.LinkCount);
            }

        }


        #region

        public static IValueWrapper<LinkItem<TValue>> AddLink<TKey, TValue>(
            ConcurrentDictionary<
                TKey,
                IValueWrapper<LinkItem<TValue>>
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
                    addValueFactory: (k) => LinkItem<TValue>.CreateOneLinkWrapper(valueFactorFunc(k)),
                    updateValueFactory: (k, e) => e.Value.IncrementCounterWrapper(updateValueFunc(k, e.Value.Value))
                    );
        }

        public static void RemoveLink<TKey, TValue>(
            ConcurrentDictionary<
                TKey,
                IValueWrapper<LinkItem<TValue>>
                >
            dictionary,
            TKey key,
            out RemoveResult<LinkItem<TValue>> result
            )
            where TKey : notnull
        {
            var changeCounterResult = dictionary
                .AddOrUpdate(
                    key: key,
                    addValueFactory: static (k) => LinkItem<TValue>.CreateZeroLinkWrapper(),
                    updateValueFactory: static (k, e) => e.Value.DecrementCounterWrapper()
                    );

            //Remove item if link count = 0
            //if (result.Value.LinkCount != 0)
            //{
            //    return false;
            //}

            dictionary.RemoveIf(
                key: key,
                removeIfFunc: static e => e.Value.LinkCount == 0,
                out result
                );
        }

        public static void RemoveLink<TKey, TValue, TParam>(                    
            ConcurrentDictionary<                    
                TKey,                    
                IValueWrapper<LinkItem<TValue>>                    
                > 
            dictionary,
            TKey key,
            TParam param,
            out RemoveResult<LinkItem<TValue>> result
            )
            where TKey : notnull
        {
            var changeCounterResult = dictionary
                .AddOrUpdate(
                    key: key,
                    addValueFactory: static (k) => LinkItem<TValue>.CreateZeroLinkWrapper(),
                    updateValueFactory: static (k,e) => e.Value.DecrementCounterWrapper()
                    );

            //Remove item if link count = 0
            //if (result.Value.LinkCount != 0)
            //{
            //    return false;
            //}

            dictionary.RemoveIf(
                key: key,
                removeIfFunc: static (e, p) => e.Value.LinkCount == 0,
                param: param,
                out result
                );
        }

        #endregion
    }
}