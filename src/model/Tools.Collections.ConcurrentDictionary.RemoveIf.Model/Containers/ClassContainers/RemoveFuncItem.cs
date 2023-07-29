using System;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers
{
    public class RemoveFuncItem<TValue>
        : IValueWrapper<TValue>,
        IEquatable<IValueWrapper<TValue>>,
        IEquatable<RealItemWrapper<TValue>>
    {
        public TValue Value => throw new NotSupportedException(nameof(Value));
        private readonly Func<IValueWrapper<TValue>, bool> _needRemoveExpression;
        internal TValue CaptureEqualsItem { set; get; } = default;


        public RemoveFuncItem(
            Func<IValueWrapper<TValue>, bool> func
            )
        {
            _needRemoveExpression = func;
        }


        public virtual bool Process(RealItemWrapper<TValue> item)
        {
            CaptureEqualsItem = item.Value;
            return _needRemoveExpression(item);
        }

        #region

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case RealItemWrapper<TValue> item:
                    return Equals(item);

                default:
                    return false;
            }
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return "RemoveFuncItem";
        }


        public bool Equals(IValueWrapper<TValue> other)
        {
            switch (other)
            {
                case RealItemWrapper<TValue> item:
                    return Equals(item);

                default:
                    return false;
            }
        }

        public bool Equals(RealItemWrapper<TValue> other)
        {
            return Process(other);
        }

        #endregion
    }
}