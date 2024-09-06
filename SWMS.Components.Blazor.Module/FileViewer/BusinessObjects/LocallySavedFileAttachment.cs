namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

public class LocallySavedFileAttachment : ViewableFileAttachment
{
    public virtual string FilePath { get; set; }


    #region ViewableFileAttachment
    public override string FileName => FilePath?.Split("/").Last() ?? "New File";

    public override byte[] Bytes
    {
        get
        {
            if (string.IsNullOrEmpty(FilePath))
                return null;

            try
            {
                return File.ReadAllBytes(FilePath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    #endregion
}