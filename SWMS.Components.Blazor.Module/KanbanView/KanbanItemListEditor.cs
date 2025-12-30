using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using SWMS.Components.Blazor.Module.KanbanView.Models;
using System.Collections;
using System.ComponentModel;

namespace SWMS.Components.Blazor.Module.KanbanView;

[ListEditor(typeof(IKanbanItem))]
public class KanbanItemListEditor : ListEditor, IComponentContentHolder
{
    private RenderFragment _componentContent;

    public KanbanBoardModel ComponentModel { get; private set; }

    public RenderFragment ComponentContent
    {
        get
        {
            _componentContent ??= ComponentModelObserver.Create(
                ComponentModel,
                ComponentModel.GetComponentContent()
            );
            return _componentContent;
        }
    }

    public KanbanItemListEditor(IModelListView model) : base(model) { }

    protected override object CreateControlsCore()
    {
        ComponentModel = new KanbanBoardModel();
        return ComponentModel;
    }

    protected override void AssignDataSourceToControl(object dataSource)
    {
        if (ComponentModel is not null)
        {
            UpdateDataSource(dataSource);

            if (dataSource is IBindingList bindingList)
            {
                bindingList.ListChanged += BindingList_ListChanged;
            }
        }
    }

    private void BindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
        UpdateDataSource(DataSource);
    }

    private void UpdateDataSource(object dataSource)
    {
        if (ComponentModel is not null)
        {
            ComponentModel.Data = (dataSource as IEnumerable)?.OfType<IKanbanItem>().ToList() ?? [];
        }
    }

    public override void BreakLinksToControls()
    {
        AssignDataSourceToControl(null);
        base.BreakLinksToControls();
    }

    public override void Refresh() => UpdateDataSource(DataSource);

    public override SelectionType SelectionType => SelectionType.None;
    public override IList GetSelectedObjects() => Array.Empty<object>();
}
