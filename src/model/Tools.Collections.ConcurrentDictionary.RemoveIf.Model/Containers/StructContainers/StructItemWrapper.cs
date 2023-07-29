using System;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.StructContainers
{
    public struct StructItemWrapper<TValue>
        : IEquatable<StructItemWrapper<TValue>>
    {
        private readonly bool _isValue;
        public TValue Value { get; }

        private readonly Func<StructItemWrapper<TValue>, bool> _needRemoveExpression;
        //internal TValue CaptureEqualsItem { set; get; }


        public StructItemWrapper(TValue value)
        {
            _isValue = true;
            Value = value;
            _needRemoveExpression = null;
            //CaptureEqualsItem = default;
        }

        public StructItemWrapper(
            Func<StructItemWrapper<TValue>, bool> needRemoveExpression
            )
        {
            _isValue = false;
            Value = default;
            _needRemoveExpression = needRemoveExpression;
            //CaptureEqualsItem = default;
        }


        #region

        public override int GetHashCode()
        {
            if (_isValue)
                return Value.GetHashCode();

            throw new NotSupportedException();
        }
        public override string ToString()
        {
            if (_isValue)
                return Value.ToString();

            return "StructItemWrapper";
        }
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case StructItemWrapper<TValue> item:
                    return Equals(item);

                default:
                    return false;
            }
        }

        public bool Equals(StructItemWrapper<TValue> other)
        {
            switch (_isValue)
            {
                case true: 
                    {
                        switch (other._isValue)
                        {
                            case true: 
                                {
                                    if (Value is IEquatable<TValue> equtableValue)
                                    { 
                                        return equtableValue.Equals(other.Value);
                                    }
                                    return Value.Equals(other.Value);
                                }
                            case false:
                                {
                                    //other.CaptureEqualsItem = Value;
                                    return other._needRemoveExpression(this);
                                }
                            default: return false;
                        }
                    }
                case false: 
                    {
                        switch (other._isValue)
                        {
                            case true:
                                {
                                    //CaptureEqualsItem = other.Value;
                                    return _needRemoveExpression(other);
                                }
                            case false:
                                {
                                    return false;
                                }
                            default: return false;
                        }
                    }
                default: return false;
            }
        }

        #endregion
    }
}