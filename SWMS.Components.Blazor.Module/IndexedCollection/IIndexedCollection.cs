namespace SWMS.Components.Blazor.Module.IndexedCollection;

public interface IIndexedCollection
{
    public abstract IList<IIndexedItem> Items { get; }
}
