using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;

namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

public class DatabaseSavedFileAttachment : ViewableFileAttachment
{
    [ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
    [FileTypeFilter("AllowedFilesFilter", 1, "*.pdf", "*.png")]
    [RuleRequiredField]
    [VisibleInListView(false)]
    public virtual FileData FileAttachment { get; set; }

    #region ViewableFileAttachment
    public override string FileName => FileAttachment?.FileName ?? "New File";

    public override byte[] Bytes => FileAttachment?.Content ?? [];
    #endregion
}
