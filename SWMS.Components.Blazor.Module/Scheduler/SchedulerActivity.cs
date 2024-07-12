using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace SWMS.Components.Blazor.Module.Scheduler;

public abstract class SchedulerActivity: IXafEntityObject, IObjectSpaceLink
{
    public abstract string Summary { get; }

    public abstract SchedulerAppointment? Appointment { get; }

    public abstract SchedulerResourceCategory SupportedResourceCategory { get; }

    [NotMapped]
    public bool IsPlanned => Appointment != null;


    #region IObjectSpaceLink
    protected IObjectSpace ObjectSpace;

    IObjectSpace IObjectSpaceLink.ObjectSpace
    {
        get
        {
            return ObjectSpace;
        }
        set
        {
            ObjectSpace = value;
        }
    }
    #endregion

    #region IXafEntityObject

    [Key]
    [VisibleInListView(false)]
    [VisibleInDetailView(false)]
    [VisibleInLookupListView(false)]
    public virtual Guid ID { get; set; }

    public virtual void OnCreated() { }
    public virtual void OnSaving() { }
    public virtual void OnLoaded() { }
    #endregion
}
