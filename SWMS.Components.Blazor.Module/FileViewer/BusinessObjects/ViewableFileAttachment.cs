using DevExpress.ExpressApp.Model;
using DevExpress.Office.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.XtraRichEdit;
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

    [Browsable(false)]
    public abstract byte[] Bytes { get; }

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

    public string ConvertToBase64()
    {
        string base64 = string.Empty;

        if (Bytes == null || Bytes.Length < 1)
            return string.Empty;

        using (var ms = new MemoryStream())
        {
            string filename = FileName;
            ms.Write(Bytes, 0, Bytes.Length);
            ms.Flush();

            ms.Position = 0;
            string mimetype = "application/pdf";
            if (string.IsNullOrEmpty(filename) == false)
            {
                // if it is a docx et al need to convert it to a pdf before rendering it
                if (filename.Contains(".doc") || filename.Contains(".docx")
                    || filename.Contains(".rtf") || filename.Contains(".xml")
                    || filename.Contains(".txt"))
                {
                    using (RichEditDocumentServer reds = new RichEditDocumentServer())
                    {
                        using (var rtms = new MemoryStream())
                        {
                            reds.LoadDocument(ms, DocumentFormat.OpenXml);
                            reds.ExportToPdf(rtms);
                            rtms.Position = 0;

                            // need to override this as it will render in a pdf
                            mimetype = "application/pdf";
                            //ret = string.Format("data:{0};base64,", mimetype);
                            base64 = Convert.ToBase64String(rtms.ToArray());
                            rtms.Dispose();
                        }
                    }
                }
                else
                {
                    mimetype = GetMimeType(filename);
                    //ret = string.Format("data:{0};base64,", mimetype);
                    base64 = Convert.ToBase64String(ms.ToArray());
                }
            }

            ms.Dispose();
            return base64;
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
