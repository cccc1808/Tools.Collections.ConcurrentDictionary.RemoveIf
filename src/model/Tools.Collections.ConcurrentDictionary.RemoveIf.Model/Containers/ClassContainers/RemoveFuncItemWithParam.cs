using System;

namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model.Containers.ClassContainers
{
    public class RemoveFuncItemWithParam<TValue, TParam>
        : RemoveFuncItem<TValue>
    {
        private readonly TParam _param;
        private readonly Func<IValueWrapper<TValue>, TParam, bool> _needRemoveExpression;


        public RemoveFuncItemWithParam(
            Func<IValueWrapper<TValue>, TParam, bool> func,
            TParam param
            ) : base(null)
        {
            _needRemoveExpression = func;
            _param = param;
        }


        public override bool Process(RealItemWrapper<TValue> item)
        {
            CaptureEqualsItem = item.Value;
            return _needRemoveExpression(item, _param);
        }
    }
}