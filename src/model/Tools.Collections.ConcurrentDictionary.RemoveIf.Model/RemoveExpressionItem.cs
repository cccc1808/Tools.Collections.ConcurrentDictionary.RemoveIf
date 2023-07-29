using System;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model
{
    internal sealed class RemoveExpressionItem<TValue>
        : IValueWrapper<TValue>
    {
        public TValue Value => default(TValue);

        public Func<IValueWrapper<TValue>, bool> NeedRemoveExpression { private set; get; }
        public TValue CaptureEqualsItem { internal set; get; }


        public RemoveExpressionItem(Func<IValueWrapper<TValue>, bool> func)
        {
            NeedRemoveExpression = func;
        }


        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case IValueWrapper<TValue> item:
                    CaptureEqualsItem = item.Value;
                    return NeedRemoveExpression(item);

                default:
                    return false;
            }
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
