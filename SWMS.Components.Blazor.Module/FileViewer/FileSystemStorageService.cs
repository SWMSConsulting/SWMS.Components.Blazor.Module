namespace SWMS.Components.Blazor.Module.FileViewer;

public class FileSystemStorageService
{
    public static int ReadBytesSize = 0x1000;
    public static string FileSystemStoreLocation
    {
        get
        {
            var path = Environment.GetEnvironmentVariable("FILE_DATA_PATH");
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileData");
            }
            Console.WriteLine($"FileSystemStoreLocation: {path}");
            return path;
        }
    }

    public static void CopyFileToStream(string sourceFileName, Stream destination)
    {
        if (string.IsNullOrEmpty(sourceFileName) || destination == null) return;
        using (Stream source = File.OpenRead(sourceFileName))
            CopyStream(source, destination);
    }
    public static void OpenFileWithDefaultProgram(string sourceFileName)
    {
        //Guard.ArgumentNotNullOrEmpty(sourceFileName, "sourceFileName");

        System.Diagnostics.Process process = new System.Diagnostics.Process();

        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = sourceFileName;
        process.Start();
    }
    public static void CopyStream(Stream source, Stream destination)
    {
        if (source == null || destination == null) return;
        byte[] buffer = new byte[ReadBytesSize];
        int read = 0;
        while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            destination.Write(buffer, 0, read);
    }

    public static string ConvertToBase64(string filePath)
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(bytes);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}
