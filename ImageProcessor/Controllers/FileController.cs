using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImageProcessor.Services;

namespace ImageProcessor.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var fileName = await _fileService.UploadFile(file);
            return Ok(fileName);
        }
    }
}
