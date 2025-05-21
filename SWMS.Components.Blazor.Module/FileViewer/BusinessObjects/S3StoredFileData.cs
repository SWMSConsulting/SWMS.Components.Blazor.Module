using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using SWMS.Components.Blazor.Module.FileViewer.Services;
using System.IO;


namespace SWMS.Components.Blazor.Module.FileViewer.BusinessObjects;

[DefaultProperty(nameof(FileName))]
[NavigationItem("test")]
public class S3StoredFileData : BaseObject, IFileData, IEmptyCheckable
{
    public virtual int Size { get; set; }

    public virtual string FileName { get; set; }

    private byte[]? content = null;

    public byte[] Content
    {
        get
        {
            if (content == null)
            {
                // Load the file from S3 if not already loaded
                try
                {
                    var tempStream = S3StorageService.LoadFileFromS3(RealFileName);
                    content = new byte[tempStream.Length];
                    tempStream.Read(content, 0, content.Length);
                }
                catch (Exception exc)
                {
                    throw new UserFriendlyException($"Error loading file from S3: {exc.Message}", exc);
                }
            }
            return content;
        }
    }

    private Stream tempSourceStream;
    public string RealFileName
    {
        get
        {
            if (ID != Guid.Empty && !string.IsNullOrEmpty(FileName))
            {
                var fileExtension = Path.GetExtension(FileName);
                return $"{ID}{fileExtension}";
            }

            return null;
        }
    }
    protected virtual void SaveFileToStore()
    {
        if (string.IsNullOrEmpty(RealFileName) || TempSourceStream == null)
        {
            throw new UserFriendlyException("File name and stream are not valid.");
        }

        try
        {
            // Reset the position of the stream to the beginning
            if (TempSourceStream.CanSeek)
            {
                TempSourceStream.Position = 0;
            }
            else
            {
                throw new UserFriendlyException("Stream is not seekable.");
            }
            // Use the S3 service to upload the file
            using (var uploadStream = new MemoryStream())
            {
                TempSourceStream.CopyTo(uploadStream);
                uploadStream.Position = 0;
                S3StorageService.UploadFileToS3(uploadStream, RealFileName);
            }

            // Optionally, you can set the file size after upload
            Size = (int)TempSourceStream.Length;
        }
        catch (Exception exc)
        {
            throw new UserFriendlyException($"Error saving file to S3: {exc.Message}", exc);
        }

    }
    public override void OnSaving()
    {
        base.OnSaving();
        if (!ObjectSpace.IsObjectToDelete(this))
        {
            //Guard.ArgumentNotNullOrEmpty(S3StorageService.FileSystemStoreLocation, "FileSystemStoreLocation");
            SaveFileToStore();
        }
        else
        {
            Clear();
        }
    }

    #region IFileData Members
    public void Clear()
    {
        if (string.IsNullOrEmpty(RealFileName))
        {
            return;
        }

        try
        {
            S3StorageService.DeleteFileFromS3(RealFileName);
            FileName = string.Empty;
            Size = 0;
        }
        catch (DirectoryNotFoundException exc)
        {
            throw new UserFriendlyException(exc);
        }
    }

    [Browsable(false)]
    [NotMapped]
    public Stream TempSourceStream
    {
        get { return tempSourceStream; }
        set
        {
            //Michael: The original Stream might be closed after a while (on the web too - T160753)
            if (value == null)
            {
                tempSourceStream = null;
            }
            else
            {
                if (value.Length > int.MaxValue)
                    throw new UserFriendlyException("File is too long");

                using (var temp = new MemoryStream())
                {
                    // Copy the source stream into the temporary memory stream
                    FileHelperService.CopyStream(value, temp);
                    tempSourceStream = new MemoryStream(temp.ToArray());
                    tempSourceStream.Position = 0;
                }
            }
        }
    }

        
    //Dennis: Fires when uploading a file.
    void IFileData.LoadFromStream(string fileName, Stream source)
    {
            //Dennis: When assigning a new file we need to save the name of the old file to remove it from the store in the future.
            if (fileName != FileName)
            {// updated, old code was: if (string.IsNullOrEmpty(tempFileName))
             //tempFileName = RealFileName;
            }
            FileName = fileName;
            TempSourceStream = source;
            Size = (int)TempSourceStream.Length;
            ObjectSpace.SetModified(this);
    }

    //Dennis: Fires when saving or opening a file.
    void IFileData.SaveToStream(Stream destination)
    {
        try
        {
            var tempStream = S3StorageService.LoadFileFromS3(RealFileName);
            FileHelperService.CopyStream(tempStream, destination);

        }
        catch (Exception exc)
        {
            throw new UserFriendlyException($"Error loading file from S3: {exc.Message}", exc);
        }
    }
    #endregion

    #region IEmptyCheckable Members
    public bool IsEmpty
    {
        //T153149
        get { return FileDataHelper.IsFileDataEmpty(this) || !(TempSourceStream != null || S3StorageService.FileExists(RealFileName)); }
    }
    #endregion
}
