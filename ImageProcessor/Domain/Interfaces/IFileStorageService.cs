﻿namespace ImageProcessor.Domain.Interfaces
{
    public interface IFileStorageService
    {
        public Task<string> UploadFile(IFormFile file, string id);
        public string GetURL(string filePath);
    }
}
