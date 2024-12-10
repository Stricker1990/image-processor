using Azure.Storage.Blobs;
using ImageProcessor.Domain.Interfaces;

namespace ImageProcessor.Services
{
    public class FileSorageService : IFileStorageService
    {
        private static readonly string CONNECTION_STRING = "UseDevelopmentStorage=true";
        private static readonly string CONTAINER_NAME = "files-container";

        private readonly BlobContainerClient _blobClient;

        public FileSorageService()
        {
            _blobClient = new BlobContainerClient(CONNECTION_STRING, CONTAINER_NAME);
        }
        public async Task<string> UploadFile(IFormFile file, string id)
        {
            var fileName = $"uploaded/{id}_{file.FileName}";
            await _blobClient.CreateIfNotExistsAsync();

            var stream = file.OpenReadStream();
            await _blobClient.UploadBlobAsync(fileName, stream);

            return fileName;
        }
    }
}
