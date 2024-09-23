using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System.ComponentModel;

namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

public class LocallySavedFileAttachment : ViewableFileAttachment
{
    [EditorAlias(nameof(FilePathStringEditor))]
    public virtual string FilePath { get; set; }

    [Browsable(false)]
    [RuleFromBoolProperty("FileExists", DefaultContexts.Save, "File does not exist", UsedProperties = "FilePath")]
    public bool FileExists => File.Exists(FilePath);


    #region ViewableFileAttachment
    public override string FileName => Path.GetFileName(FilePath) ?? "New File";

    public override byte[] Bytes
    {
        get
        {
            if (string.IsNullOrEmpty(FilePath))
                return [];

            try
            {
                return File.ReadAllBytes(FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return [];
            }
        }
    }
    #endregion
}