using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System.Collections.ObjectModel;

namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

[NavigationItem("System Settings")]
public class FileAttachmentType : BaseObject
{
    [RuleRequiredField]
    public virtual string Name { get; set; }

    [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
    public virtual IList<CustomDBSavedFileAttachment> FileAttachments { get; set; } = new ObservableCollection<CustomDBSavedFileAttachment>();
}
