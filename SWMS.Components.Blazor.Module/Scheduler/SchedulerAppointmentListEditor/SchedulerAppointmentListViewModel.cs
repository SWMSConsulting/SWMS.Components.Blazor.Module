
using DevExpress.ExpressApp.Blazor.Components.Models;
using System.Collections.ObjectModel;

namespace SWMS.Components.Blazor.Module.Scheduler.SchedulerAppointmentListEditor;

public class SchedulerAppointmentListViewModel : ComponentModelBase
{
    public ObservableCollection<SchedulerAppointment> Appointments
    {
        get => GetPropertyValue<ObservableCollection<SchedulerAppointment>>();
        set => SetPropertyValue(value);
    }
    public ObservableCollection<SchedulerResource> Resources
    {
        get => GetPropertyValue<ObservableCollection<SchedulerResource>>();
        set => SetPropertyValue(value);
    }
    public ObservableCollection<SchedulerActivity> PlannableActivities
    {
        get => GetPropertyValue<ObservableCollection<SchedulerActivity>>();
        set => SetPropertyValue(value);
    }

    public void Refresh() => RaiseChanged();
    public void OnDragToPlanActivity(DragToPlanActivityEventArgs args) => DragToPlanActivity?.Invoke(this, args);
    public event EventHandler<DragToPlanActivityEventArgs> DragToPlanActivity;
}
public class DragToPlanActivityEventArgs : EventArgs
{
    public DragToPlanActivityEventArgs(SchedulerResource resource, SchedulerActivity plannableActivity, DateTime startOn)
    {
        Resource = resource;
        PlannableActivity = plannableActivity;
        StartOn = startOn;
    }
    public SchedulerResource Resource { get; }
    public SchedulerActivity PlannableActivity { get; }
    public DateTime StartOn { get; }
}