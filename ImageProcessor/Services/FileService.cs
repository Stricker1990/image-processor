using Azure.Storage.Blobs;

namespace ImageProcessor.Services
{
    public class FileService : IFileService
    {
        private static readonly string CONNECTION_STRING = "UseDevelopmentStorage=true";
        private static readonly string CONTAINER_NAME = "files-container";

        private readonly BlobContainerClient _blobClient;

        public FileService()
        {
            _blobClient = new BlobContainerClient(CONNECTION_STRING, CONTAINER_NAME);
        }
        public async Task<string> UploadFile(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString();
            await _blobClient.CreateIfNotExistsAsync();

            var stream = file.OpenReadStream();
            await _blobClient.UploadBlobAsync(fileName, stream);

            return fileName;
        }
    }
}
