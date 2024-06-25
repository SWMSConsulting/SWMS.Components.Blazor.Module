namespace SWMS.Components.Blazor.Module.IndexedCollection;

public interface IIndexedItem
{
    public bool IsEditable { get; }

    public int Index { get; set; }

    public IIndexedCollection Collection { get; }
}
