using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;

namespace SWMS.Components.Blazor.Module.IndexedCollection;
public class IndexedItemListViewController : ObjectViewController<ListView, IIndexedItem>
{
    public IndexedItemListViewController()
    {
    }
    protected override void OnViewControlsCreated()
    {
        base.OnViewControlsCreated();

        if (View.Editor is DxGridListEditor)
        {
            var gridListEditor = View.Editor as DxGridListEditor;
            gridListEditor.RowClickMode = RowClickMode.ProcessOnDouble;
            var dxDataGridAdapter = gridListEditor.GetGridAdapter();

            var objectSpace = View.ObjectSpace;
            if (dxDataGridAdapter == null)
            {
                return;
            }

            foreach (var columnModel in dxDataGridAdapter.GridDataColumnModels)
            {
                if (columnModel.FieldName == nameof(IIndexedItem.Index))
                {
                    columnModel.VisibleIndex = 0;
                    columnModel.SortIndex = 0;
                    columnModel.Width = "100px";
                    columnModel.CellDisplayTemplate = IndexEditorTemplate.Create(objectSpace);
                }
            }
        }
    }
}
