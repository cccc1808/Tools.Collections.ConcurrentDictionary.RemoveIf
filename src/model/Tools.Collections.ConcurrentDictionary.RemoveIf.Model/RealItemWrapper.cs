namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model
{
    public sealed class RealItemWrapper<TValue>
        : IValueWrapper<TValue>
    {
        public TValue Value { private set; get; }


        public RealItemWrapper(TValue value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;

                case RealItemWrapper<TValue> item:
                    return Value.Equals(item.Value);

                case RemoveExpressionItem<TValue> item:
                    item.CaptureEqualsItem = Value;
                    return item.NeedRemoveExpression(this);

                default:
                    return false;
            }
        }
    }
}
