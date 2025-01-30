namespace SWMS.Components.Blazor.Module.FileViewer.Services;

public class FileSystemStorageService
{
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

}
