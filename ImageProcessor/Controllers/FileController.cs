using Microsoft.AspNetCore.Mvc;
using ImageProcessor.Domain.Interfaces;

namespace ImageProcessor.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private IFileStorageService _fileService;
        private ITaskService _taskService;
        public FileController(IFileStorageService fileService, ITaskService taskService)
        {
            _fileService = fileService;
            _taskService = taskService;
        }


        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            var storagePath = await _fileService.UploadFile(file);
            var task = await _taskService.CreateTaskAsync(file.FileName, storagePath);
            return Ok(storagePath);
        }
    }
}
