using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.XtraRichEdit;

namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

public class CustomDBSavedFileAttachment : ViewableFileAttachment
{
    [ExpandObjectMembers(ExpandObjectMembers.Never), ImmediatePostData]
    [FileTypeFilter("AllowedFilesFilter", 1, "*.pdf", "*.png")]
    [RuleRequiredField]
    [VisibleInListView(false)]
    public virtual FileData FileAttachment { get; set; }
    //public virtual FileSystemStoreObject FileAttachment { get; set; }

    #region ViewableFileAttachment
    public override string FileName => FileAttachment?.FileName ?? "New File";

    public override string ConvertToBase64()
    {
        if (FileAttachment == null || FileAttachment.FileName == null || FileAttachment.Size < 1)
            return string.Empty;

        string base64 = string.Empty;
        using (var ms = new MemoryStream())
        {
            string filename = FileAttachment.FileName.ToLower();
            FileAttachment.SaveToStream(ms);
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
    #endregion
}
