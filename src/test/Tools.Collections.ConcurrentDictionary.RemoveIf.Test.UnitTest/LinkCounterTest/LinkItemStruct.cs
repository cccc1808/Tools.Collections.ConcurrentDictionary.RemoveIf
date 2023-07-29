using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.StructContainers;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Test.UnitTest.LinkCounterTest
{
    public record struct LinkItemStruct<T>
    {
        #region

        public int LinkCount { init; get; }
        public T Value { init; get; }
        public bool HaveValue { init; get; }

        #endregion


        #region

        public static LinkItemStruct<T> CreateZeroLink()
        {
            return new LinkItemStruct<T>()
            {
                LinkCount = 0,
                HaveValue = false
            };
        }
        public static StructItemWrapper<LinkItemStruct<T>> CreateZeroLinkWrapper()
        {
            return new StructItemWrapper<LinkItemStruct<T>>(
                CreateZeroLink()
                );
        }


        public static LinkItemStruct<T> CreateOneLink(T value)
        {
            return new LinkItemStruct<T>()
            {
                Value = value,
                LinkCount = 1,
                HaveValue = true
            };
        }
        public static StructItemWrapper<LinkItemStruct<T>> CreateOneLinkWrapper(T value)
        {
            return new StructItemWrapper<LinkItemStruct<T>>(
                CreateOneLink(value)
                );
        }


        public LinkItemStruct<T> IncrementCounter(T value)
        {
            return new LinkItemStruct<T>()
            {
                LinkCount = LinkCount + 1,
                Value = value,
                HaveValue = true
            };
        }
        public StructItemWrapper<LinkItemStruct<T>> IncrementCounterWrapper(T value)
        {
            return new StructItemWrapper<LinkItemStruct<T>>(
                IncrementCounter(value)
                );
        }


        public LinkItemStruct<T> DecrementCounter()
        {
            if (
                HaveValue
                && LinkCount > 0
                )
            {
                return new LinkItemStruct<T>()
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
        public StructItemWrapper<LinkItemStruct<T>> DecrementCounterWrapper()
        {
            return new StructItemWrapper<LinkItemStruct<T>>(
                DecrementCounter()
                );
        }

        #endregion
    }
}