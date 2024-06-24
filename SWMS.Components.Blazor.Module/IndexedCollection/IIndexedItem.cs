namespace SalesDashboard.Module.BusinessObjects
{
    public interface IIndexedItem
    {
        public int Index { get; set; }

        public IIndexedCollection Collection { get; }
    }
}
