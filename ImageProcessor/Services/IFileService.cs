namespace ImageProcessor.Services
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);
    }
}
