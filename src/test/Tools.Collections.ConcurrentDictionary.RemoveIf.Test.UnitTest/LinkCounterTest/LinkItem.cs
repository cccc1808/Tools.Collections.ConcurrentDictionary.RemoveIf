using Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Test.UnitTest.LinkCounterTest
{
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
}