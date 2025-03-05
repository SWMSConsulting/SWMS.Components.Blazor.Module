using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace SWMS.Components.Blazor.Module.FileViewer
{
    public class S3StorageService
    {
        public static int ReadBytesSize = 0x1000;


        // File upload method
        public static void UploadFileToS3(Stream fileStream, string fileName)
        {
            if (fileStream == null || string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File stream or file name is invalid.");
            }

            try
            {
                var awsAccessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
                var awsSecretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
                var serviceUrl = Environment.GetEnvironmentVariable("S3_SERVICE_URL");
                var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
                Console.WriteLine($"Access Key: {awsAccessKey}");
                Console.WriteLine($"Secret Key: {awsSecretKey}");
                Console.WriteLine($"Service URL: {serviceUrl}");
                Console.WriteLine($"Bucket Name: {bucketName}");
                // Ensure environment variables are set
                if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(bucketName))
                {
                    throw new InvalidOperationException("S3 credentials or bucket information is missing.");
                }

                // Create S3 client
                using var s3Client = new AmazonS3Client(
                    new BasicAWSCredentials(awsAccessKey, awsSecretKey),
                    new AmazonS3Config
                    {
                        ServiceURL = serviceUrl,
                        ForcePathStyle = true  // Use path-style addressing for custom domains
                    }
                );

                // Prepare the upload request
                var uploadRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = fileStream,
                    ContentType = "application/octet-stream", // Change if needed
                };

                // Upload the file
                var response = s3Client.PutObjectAsync(uploadRequest).Result;
                Console.WriteLine($"File '{fileName}' uploaded successfully to bucket '{bucketName}'. RequestId: {response.ResponseMetadata.RequestId}");
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
                var awsAccessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
                var awsSecretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
                var serviceUrl = Environment.GetEnvironmentVariable("S3_SERVICE_URL");
                var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");

                // Ensure environment variables are set
                if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(serviceUrl) || string.IsNullOrEmpty(bucketName))
                {
                    throw new InvalidOperationException("S3 credentials or bucket information is missing.");
                }
                // Create S3 client
                using var s3Client = new AmazonS3Client(
                    new BasicAWSCredentials(awsAccessKey, awsSecretKey),
                    new AmazonS3Config
                    {
                        ServiceURL = serviceUrl,
                        ForcePathStyle = true  // Use path-style addressing for custom domains
                    }
                );
                // Prepare the delete request
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName
                };
                var response = s3Client.DeleteObjectAsync(deleteRequest).Result;
                Console.WriteLine($"File '{fileName}' deleted successfully from bucket '{bucketName}'. RequestId: {response.ResponseMetadata.RequestId}");
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

}
