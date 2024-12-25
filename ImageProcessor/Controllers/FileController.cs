using Microsoft.AspNetCore.Mvc;
using ImageProcessor.Domain.Interfaces;
using ImageProcessor.DTO;
using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Controllers
{
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private IFileStorageService _fileService;
        private ITaskService _taskService;
        private IMessagesService _messagesService;
        public FileController(IFileStorageService fileService, ITaskService taskService, IMessagesService messageService)
        {
            _fileService = fileService;
            _taskService = taskService;
            _messagesService = messageService;
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var id = Guid.NewGuid().ToString();
            var storagePath = await _fileService.UploadFile(file, id);
            var task = await _taskService.CreateTaskAsync(id, file.FileName, storagePath);
            await _messagesService.PublishMessage(new Message() { Id = id, BlobName = storagePath});
            return Ok(task.id);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskState(string taskId)
        {
            var task = await _taskService.GetTaskAsync(taskId);
            var fileURL = _fileService.GetURL(task.ProcessedFilePath);
            TaskStateDTO result = new()
            {
                State = task.State,
                FileName = task.FileName,
                FileURL = fileURL

            };
            return Ok(result);
        }

        [HttpPost]
        [Route("/process")]
        public async Task<IActionResult> ProcessImage(string taskId)
        {
            var task = await _taskService.GetTaskAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task = task with { State = TaskState.InProgress};
            task = await _taskService.UpdateTaskAsync(task);

            var processedFile = await _fileService.RotateFile(task);

            task = task with { State = TaskState.Done, ProcessedFilePath = processedFile };
            task = await _taskService.UpdateTaskAsync(task);

            return Ok();
        }

        [HttpPost]
        [Route("/error")]
        public async Task<IActionResult> SetErrorState(string taskId)
        {
            var task = await _taskService.GetTaskAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task = task with { State = TaskState.Error };
            await _taskService.UpdateTaskAsync(task);

            return Ok();
        }
    }
}
