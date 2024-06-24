namespace SWMS.Components.Blazor.Module
{
    public interface IIndexedItem
    {
        public int Index { get; set; }

        public IIndexedCollection Collection { get; }
    }
}
