using DevExpress.Persistent.Base;
using System.ComponentModel;


namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

[DefaultProperty(nameof(FileName))]
[NavigationItem("test")]
public class S3StoredFileAttachment : ViewableFileAttachment
{
    
    public virtual S3StoredFileData FileData { get; set; }


    #region ViewableFileAttachment
    public override string FileName => FileData?.FileName ?? "New File";

    public override byte[] Bytes => FileData?.Content ?? [];
    #endregion
}
