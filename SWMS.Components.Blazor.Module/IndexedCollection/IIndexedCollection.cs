namespace SWMS.Components.Blazor.Module
{
    public interface IIndexedCollection
    {
        public abstract IList<IIndexedItem> Items { get; }
    }
}
