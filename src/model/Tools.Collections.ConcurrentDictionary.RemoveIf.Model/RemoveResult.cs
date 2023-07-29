namespace Tools.Collections.ConcurrentDictionary.RemoveIf.Model
{
    public struct RemoveResult<TData>
    {
        public bool IsRemoved { private set; get; }
        public TData Data { private set; get; }


        public RemoveResult(
            bool isRemoved,
            TData data
            )
        {
            IsRemoved = isRemoved;
            Data = data;
        }
    }
}
