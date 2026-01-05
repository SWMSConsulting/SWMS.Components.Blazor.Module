namespace SWMS.Components.Blazor.Module.KanbanView.Models;

public interface IKanbanItem
{
    public string KanbanTitle { get; }
    public string KanbanDescription { get; }
    public IKanbanColumn KanbanColumn { get; set; }

    bool IsDragEnabled { get; }

    bool CanDropTo(IKanbanColumn targetColumn);
}
