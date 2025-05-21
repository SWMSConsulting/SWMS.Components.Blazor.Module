using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace SWMS.Components.Blazor.Module.FileViewer.Services;

public class S3StorageService
{
    public static int ReadBytesSize = 0x1000;

    private static string BucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? "";

    private static AmazonS3Client GetClient()
    {
        var awsAccessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
        var awsSecretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
        var serviceUrl = Environment.GetEnvironmentVariable("S3_SERVICE_URL");
        Console.WriteLine($"Access Key: {awsAccessKey}");
        Console.WriteLine($"Secret Key: {awsSecretKey}");
        Console.WriteLine($"Service URL: {serviceUrl}");
        Console.WriteLine($"Bucket Name: {BucketName}");

        if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(BucketName))
        {
            throw new ArgumentNullException("S3 credentials or bucket information is missing.");
        }

        return new AmazonS3Client(
            new BasicAWSCredentials(awsAccessKey, awsSecretKey),
            new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true  // Use path-style addressing for custom domains
            }
        );
    }

    public static void UploadFileToS3(Stream fileStream, string fileName)
    {
        if (fileStream == null || string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File stream or file name is invalid.");
        }

        try
        {
            using var s3Client = GetClient();

            var uploadRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = "application/octet-stream", // Change if needed
            };

            var response = s3Client.PutObjectAsync(uploadRequest).Result;
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Error uploading file to S3: {response.HttpStatusCode}");
            }

            Console.WriteLine($"File '{fileName}' uploaded successfully to bucket '{BucketName}'.");
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
            throw;
        }
    }
    
    public static void DeleteFileFromS3(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.");
        }
        try
        {
            using var s3Client = GetClient();

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = fileName
            };
            var response = s3Client.DeleteObjectAsync(deleteRequest).Result;
            Console.WriteLine($"File '{fileName}' deleted successfully from bucket '{BucketName}'.");
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
            throw;
        }
    }

    public static MemoryStream LoadFileFromS3(string filename)
    {
        if (string.IsNullOrEmpty(filename))
        {
            throw new ArgumentException("File name cannot be null or empty.");
        }
        try
        {
            using var s3Client = GetClient();
            var getRequest = new GetObjectRequest
            {
                BucketName = BucketName,
                Key = filename
            };
            using var response = s3Client.GetObjectAsync(getRequest).Result;
            using var responseStream = response.ResponseStream;
            var memoryStream = new MemoryStream();
            responseStream.CopyTo(memoryStream);
            memoryStream.Position = 0; // Reset the position of the stream
            return memoryStream;
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"AWS S3 error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
            throw;
        }
    }
}
