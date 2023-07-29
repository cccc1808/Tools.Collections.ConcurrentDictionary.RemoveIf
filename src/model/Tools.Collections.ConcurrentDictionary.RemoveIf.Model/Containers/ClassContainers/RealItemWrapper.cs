using System;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers
{
    public class RealItemWrapper<TValue>
        : IValueWrapper<TValue>,
        IEquatable<IValueWrapper<TValue>>,
        IEquatable<RealItemWrapper<TValue>>,
        IEquatable<RemoveFuncItem<TValue>>
    {
        public TValue Value { get; }


        public RealItemWrapper(TValue value)
        {
            Value = value;
        }


        #region

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case RealItemWrapper<TValue> realWrapper:
                    {
                        return Equals(realWrapper);
                    }
                case RemoveFuncItem<TValue> removeWrapper:
                    {
                        return Equals(removeWrapper);
                    }
            }

            return false;
        }
        public override string ToString()
        {
            return Value.ToString();
        }


        public bool Equals(IValueWrapper<TValue> other)
        {
            switch (other)
            {
                case RealItemWrapper<TValue> realWrapper:
                    {
                        return Equals(realWrapper);
                    }
                case RemoveFuncItem<TValue> removeWrapper:
                    {
                        return Equals(removeWrapper);
                    }
            }

            return false;
        }

        public bool Equals(RealItemWrapper<TValue> other)
        {
            if (Value is IEquatable<TValue> equatableValue)
            {
                return equatableValue.Equals(other.Value);
            }

            return Value.Equals(other.Value);
        }

        public bool Equals(RemoveFuncItem<TValue> other)
        {
            return other.Process(this);
        }

        #endregion
    }
}
