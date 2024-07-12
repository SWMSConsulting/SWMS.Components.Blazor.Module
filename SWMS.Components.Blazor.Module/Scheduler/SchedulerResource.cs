using DevExpress.ExpressApp;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace SWMS.Components.Blazor.Module.Scheduler;

public abstract class SchedulerResource : IResource, IXafEntityObject, IObjectSpaceLink
{
    [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
    public Object IdMapping => ID;

    [NotMapped]
    public abstract SchedulerResourceCategory SchedulerResourceCategory { get; }

    [NotMapped]
    public string ResourceCategoryName => SchedulerResourceCategory.CategoryName;

    [NotMapped]
    public abstract string ResourceName { get; }

    #region IResource
    public abstract IList<SchedulerAppointment> SchedulerAppointments { get; }

    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    [NotMapped]
    public String Caption { get { return ResourceName; } set { } }


    [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
    public Object Id => ID;

    [NotMapped]
    public Int32 OleColor => SchedulerResourceCategory.OleColor;

    [NotMapped]
    public Color Color => SchedulerResourceCategory.Color;

    [NotMapped]
    public string TextCssClass => "text-black"; // "text-white";
    #endregion

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

