
using Azure.Storage.Blobs;
using ImageProcessor.Domain.Interfaces;
using ImageProcessor.Domain.Entity;

using Azure.Identity;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessor.Services
{
    public class FileSorageService : IFileStorageService
    {
        private static readonly string CONTAINER_NAME = "files-container";

        private readonly BlobContainerClient _blobClient;

        public FileSorageService(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            var endpoint = config.GetValue<string>("AZURE_STORAGEBLOB_RESOURCEENDPOINT");
            if(hostEnvironment.IsDevelopment())
            {
                _blobClient = new BlobContainerClient(endpoint, CONTAINER_NAME);
                _blobClient.CreateIfNotExistsAsync().Wait();
            }
            else
            {
                var blobServiceClient = new BlobServiceClient(new Uri(endpoint), new DefaultAzureCredential());
                _blobClient = blobServiceClient.GetBlobContainerClient(CONTAINER_NAME);
            }
            
        }

        public async Task<string> UploadFile(IFormFile file, string id)
        {
            var fileName = $"uploaded/{id}_{file.FileName}";

            var stream = file.OpenReadStream();
            var res = await _blobClient.UploadBlobAsync(fileName, stream);

            return fileName;
        }

        public string GetURL(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                return "";
            }
            return new Uri(_blobClient.Uri, $"{CONTAINER_NAME}/{filePath}").ToString(); ;
        }

        public async Task<string> RotateFile(TaskEntity task)
        {
            var blobClient = _blobClient.GetBlobClient(task.InitialFilePath);

            var processedFileName = $"processed/{task.id}_{task.FileName}";
            using MemoryStream inputStream = new MemoryStream();
            await blobClient.DownloadToAsync(inputStream);

            inputStream.Position = 0;

            using Image image = await Image.LoadAsync(inputStream);
            image.Mutate(x => x.RotateFlip(RotateMode.Rotate180, FlipMode.None));

            BlobClient newBlobClient = _blobClient.GetBlobClient(processedFileName);

            using MemoryStream outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream);
            outputStream.Position = 0;

            await _blobClient.UploadBlobAsync(processedFileName, outputStream);
            return processedFileName;
        }
    }
}
