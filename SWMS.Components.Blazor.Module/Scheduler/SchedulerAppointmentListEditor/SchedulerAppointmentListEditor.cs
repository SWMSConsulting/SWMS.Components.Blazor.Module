using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp;
using Microsoft.AspNetCore.Components;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Blazor;

namespace SWMS.Components.Blazor.Module.Scheduler.SchedulerAppointmentListEditor;
public class SchedulerAppointmentListEditor : ListEditor, IComplexListEditor
{

    public static Type? SchedulerResourceType = null;
    public static Type? SchedulerActivityType = null;
    public static Type? SchedulerAppointmentType = null;

    public IObjectSpace ObjectSpace { get; set; }
    public XafApplication Application { get; set; }
    public void Setup(CollectionSourceBase collectionSource, XafApplication application)
    {
        ObjectSpace = collectionSource.ObjectSpace;
        Application = application;
    }
    public class SchedulerAppointmentListViewHolder : IComponentContentHolder
    {
        private RenderFragment componentContent;
        public SchedulerAppointmentListViewHolder(SchedulerAppointmentListViewModel componentModel)
        {
            ComponentModel =
                componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        }
        private RenderFragment CreateComponent() =>
            ComponentModelObserver.Create(ComponentModel,
                                            SchedulerAppointmentListViewRenderer.Create(ComponentModel));
        public SchedulerAppointmentListViewModel ComponentModel { get; }
        RenderFragment IComponentContentHolder.ComponentContent =>
            componentContent ??= CreateComponent();
    }
    private SchedulerAppointment[] selectedObjects = Array.Empty<SchedulerAppointment>();
    public SchedulerAppointmentListEditor(IModelListView model) : base(model) { }
    protected override object CreateControlsCore() =>
        new SchedulerAppointmentListViewHolder(new SchedulerAppointmentListViewModel());
    protected override void AssignDataSourceToControl(object dataSource)
    {
        if (Control is SchedulerAppointmentListViewHolder holder)
        {
            if (holder.ComponentModel.Appointments is IBindingList bindingList)
            {
                bindingList.ListChanged -= BindingList_ListChanged;
            }

            var appointments = (dataSource as IEnumerable)?.OfType<SchedulerAppointment>().OrderBy(i => i.StartOn);
            holder.ComponentModel.Appointments = appointments == null ?
                new ObservableCollection<SchedulerAppointment>() : new ObservableCollection<SchedulerAppointment>(appointments);

            var resources = SchedulerResourceType != null 
                ? ObjectSpace.GetObjects(SchedulerResourceType).Cast<SchedulerResource>().ToList()
                : new List<SchedulerResource>();

            holder.ComponentModel.Resources = new ObservableCollection<SchedulerResource>(resources);

            var activities = SchedulerActivityType != null 
                ? ObjectSpace.GetObjects(SchedulerActivityType).Cast<SchedulerActivity>().ToList()
                : new List<SchedulerActivity>();
            holder.ComponentModel.PlannableActivities = new ObservableCollection<SchedulerActivity>(
                activities.Where(s => !s.IsPlanned).ToList()
            );

            if (dataSource is IBindingList newBindingList)
            {
                newBindingList.ListChanged += BindingList_ListChanged;
            }
        }
    }
    protected override void OnControlsCreated()
    {
        if (Control is SchedulerAppointmentListViewHolder holder)
        {
            holder.ComponentModel.DragToPlanActivity += ComponentModel_DragToPlanActivity;
        }
        base.OnControlsCreated();
    }
    public override void BreakLinksToControls()
    {
        if (Control is SchedulerAppointmentListViewHolder holder)
        {
            holder.ComponentModel.DragToPlanActivity -= ComponentModel_DragToPlanActivity;
        }
        AssignDataSourceToControl(null);
        base.BreakLinksToControls();
    }
    public override void Refresh()
    {
        if (Control is SchedulerAppointmentListViewHolder holder)
        {
            holder.ComponentModel.Refresh();
        }
    }
    private void BindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
        Refresh();
    }
    private void ComponentModel_DragToPlanActivity(object sender,
                                            DragToPlanActivityEventArgs e)
    {

        if (Control is SchedulerAppointmentListViewHolder holder)
        {
            var activity = e.PlannableActivity as SchedulerActivity;
            var resource = e.Resource as SchedulerResource;

            if (activity == null || resource == null)
            {
                return;
            }

            if (activity.SupportedResourceCategory != resource.SchedulerResourceCategory)
            {
                MessageOptions options = new MessageOptions();
                options.Duration = 2000;
                options.Message = $"Diese Aktivität erfordert eine Resource der Kategorie {activity.SupportedResourceCategory}.";
                options.Type = InformationType.Warning;
                options.Web.Position = InformationPosition.Bottom;
                options.Win.Caption = "Achtung";
                options.Win.Type = WinMessageType.Toast;
                Application.ShowViewStrategy.ShowMessage(options);
                return;
            }

            var SchedulerAppointment = ObjectSpace.CreateObject(SchedulerAppointmentType) as SchedulerAppointment;
            if(SchedulerAppointment == null)
            {
                return;
            }
            SchedulerAppointment.StartOn = e.StartOn;
            SchedulerAppointment.EndOn = e.StartOn.AddHours(8);
            SchedulerAppointment.SchedulerResource = resource;
            SchedulerAppointment.SchedulerActivity = activity;

            ObjectSpace.CommitChanges();

            holder.ComponentModel.Appointments.Add(SchedulerAppointment);
            holder.ComponentModel.PlannableActivities.Remove(activity);

            //Refresh();
        }
    }
    public override SelectionType SelectionType => SelectionType.Full;
    public override IList GetSelectedObjects() => selectedObjects;


}