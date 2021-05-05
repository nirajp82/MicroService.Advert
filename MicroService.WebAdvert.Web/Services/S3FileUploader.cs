using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace MicroService.WebAdvert.Web
{
    public class S3FileUploader : IFileUploader
    {
        private readonly IConfiguration _configuration;

        public S3FileUploader(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<bool> UploadFileAsync(string fileName, Stream storageStream)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("Filename must be specified");

            if (storageStream.Length <= 0)
                throw new InvalidDataException("Invalid file");

            string bucketName = _configuration.GetValue<string>("ImageBucket");
            try
            {
                if (storageStream.CanSeek)
                    storageStream.Seek(0, SeekOrigin.Begin);
                using var client = new AmazonS3Client();
                PutObjectRequest putObjectRequest = new()
                {
                    AutoCloseStream = true,
                    BucketName = bucketName,
                    InputStream = storageStream,
                    Key = fileName
                };
                var response = await client.PutObjectAsync(putObjectRequest);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }
    }
}