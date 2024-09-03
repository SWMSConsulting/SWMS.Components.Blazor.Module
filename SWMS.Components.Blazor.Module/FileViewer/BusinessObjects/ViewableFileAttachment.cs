using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel;

namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

[DefaultProperty(nameof(FileName))]
public abstract class ViewableFileAttachment : BaseObject
{
    [RuleRequiredField]
    public virtual FileAttachmentType FileAttachmentType { get; set; }

    [VisibleInDetailView(false), VisibleInListView(true)]
    public abstract string FileName { get; }
    public abstract string ConvertToBase64();

    [VisibleInListView(false), VisibleInDetailView(true)]
    [ModelDefault("PropertyEditorType", "SWMS.Components.Blazor.Module.FileViewer.FileDataPropertyEditor")]
    [EditorAlias("FileDataPropertyEditor")]
    public string Content
    {
        get
        {
            var base64 = ConvertToBase64();
            if (string.IsNullOrEmpty(base64))
                return string.Empty;

            var mimetype = GetMimeType(FileName);

            return $"data:{mimetype};base64,{base64}";
        }
    }
    public static string GetMimeType(string fileName)
    {
        if (fileName.Contains(".doc") || fileName.Contains(".docx")
            || fileName.Contains(".rtf") || fileName.Contains(".xml")
            || fileName.Contains(".txt"))
        {
            return "application/pdf";
        }
        // need to Install-Package Microsoft.AspNetCore.StaticFiles
        var provider = new FileExtensionContentTypeProvider();
        string contentType;
        if (!provider.TryGetContentType(fileName, out contentType))
            contentType = "application/octet-stream";
        return contentType;
    }
}
