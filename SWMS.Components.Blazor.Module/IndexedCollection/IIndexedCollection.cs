namespace SalesDashboard.Module.BusinessObjects
{
    public interface IIndexedCollection
    {
        public abstract IList<IIndexedItem> Items { get; }
    }
}
