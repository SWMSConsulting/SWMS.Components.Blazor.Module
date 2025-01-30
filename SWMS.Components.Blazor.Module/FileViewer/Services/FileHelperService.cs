using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWMS.Components.Blazor.Module.FileViewer.Services;

public static class FileHelperService
{
    public static int ReadBytesSize = 0x1000;

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
