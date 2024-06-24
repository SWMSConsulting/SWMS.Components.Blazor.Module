namespace SWMS.Components.Blazor.Module.IndexedCollection;

public interface IIndexedItem
{
    public int Index { get; set; }

    public IIndexedCollection Collection { get; }
}
