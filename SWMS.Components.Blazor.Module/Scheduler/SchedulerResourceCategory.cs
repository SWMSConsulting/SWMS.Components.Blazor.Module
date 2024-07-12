using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp;
using System.ComponentModel.DataAnnotations;

namespace SWMS.Components.Blazor.Module.Scheduler
{
    public abstract class SchedulerResourceCategory: IXafEntityObject, IObjectSpaceLink
    {
        public string CategoryName { get; }

        public IList<SchedulerResource> SchedulerResources { get; } = new List<SchedulerResource>();

        [Browsable(false)]
        public virtual Int32 Color_Int { get; protected set; }

        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public Int32 OleColor => ColorTranslator.ToOle(Color.FromArgb(Color_Int));

        [NotMapped]
        public Color Color
        {
            get { return Color.FromArgb(Color_Int); }
            set { Color_Int = value.ToArgb(); }
        }

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
        public virtual void OnCreated()
        {
            Color = Color.White;
        }
        public virtual void OnSaving() { }
        public virtual void OnLoaded() { }
        #endregion

    }
}
