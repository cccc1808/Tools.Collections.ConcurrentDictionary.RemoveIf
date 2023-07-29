# Tools.Collections.ConcurrentDictionary.RemoveIf
## Description
Extensions method for System.Collections.Concurrent.ConcurrentDictionary  
Namespace: Tools.Collections.Concurrent.Extensions  
Use: ConcurrentDictionary<TKey, IValueWrapper<TValue>>  
Description of the mechanism of work: https://stackoverflow.com/questions/39679779/how-to-achieve-remove-if-functionality-in-net-concurrentdictionary

## Usage example:
```c#

using System.Collections.Concurrent;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model;
using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers;

public record LinkItem<T>
{
    #region

    public int LinkCount { init; get; }
    public T Value { init; get; }
    public bool HaveValue { init; get; }

    #endregion


    #region

    public static LinkItem<T> CreateZeroLink()
    {
        return new LinkItem<T>()
        {
            LinkCount = 0,
            HaveValue = false
        };
    }
    public static RealItemWrapper<LinkItem<T>> CreateZeroLinkWrapper()
    {
        return new RealItemWrapper<LinkItem<T>>(
            CreateZeroLink()
            );
    }


    public static LinkItem<T> CreateOneLink(T value)
    {
        return new LinkItem<T>()
        {
            Value = value,
            LinkCount = 1,
            HaveValue = true
        };
    }
    public static RealItemWrapper<LinkItem<T>> CreateOneLinkWrapper(T value)
    {
        return new RealItemWrapper<LinkItem<T>>(
            CreateOneLink(value)
            );
    }


    public LinkItem<T> IncrementCounter(T value)
    {
        return new LinkItem<T>()
        {
            LinkCount = LinkCount + 1,
            Value = value,
            HaveValue = true
        };
    }
    public RealItemWrapper<LinkItem<T>> IncrementCounterWrapper(T value)
    {
        return new RealItemWrapper<LinkItem<T>>(
            IncrementCounter(value)
            );
    }


    public LinkItem<T> DecrementCounter()
    {
        if (
            HaveValue
            && LinkCount > 0
            )
        {
            return new LinkItem<T>()
            {
                Value = Value,
                LinkCount = LinkCount - 1,
                HaveValue = true
            };
        }
        else
        {
            return this;
        }
    }
    public RealItemWrapper<LinkItem<T>> DecrementCounterWrapper()
    {
        return new RealItemWrapper<LinkItem<T>>(
            DecrementCounter()
            );
    }

    #endregion
}


public class LinkCounterClassTest
{
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
        }

        {
            //Remove first link to key
            RemoveLink(dictionary, key, out var removeResult1);
            //Remove second link to key and remove key Item
            RemoveLink(dictionary, key, out var removeResult2);
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

    #endregion
}

```