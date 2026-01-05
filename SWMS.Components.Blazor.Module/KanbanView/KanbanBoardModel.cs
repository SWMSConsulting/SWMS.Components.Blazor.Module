using DevExpress.ExpressApp.Blazor.Components.Models;
using SWMS.Components.Blazor.Module.KanbanView.Models;

namespace SWMS.Components.Blazor.Module.KanbanView;

public class KanbanBoardModel : ComponentModelBase
{
    public IEnumerable<IKanbanItem> Items
    {
        get => GetPropertyValue<IEnumerable<IKanbanItem>>();
        set => SetPropertyValue(value);
    }

    public IEnumerable<IKanbanColumn> Columns
    {
        get => GetPropertyValue<IEnumerable<IKanbanColumn>>();
        set => SetPropertyValue(value);
    }

    public override Type ComponentType => typeof(KanbanBoard);
}
