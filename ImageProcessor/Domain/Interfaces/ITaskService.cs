using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string fileName, string originalStoragePath);
        TaskEntity StartProcessingTask(string id);
        TaskEntity FinishProcessingTask(string id, string processedStoragePath);
    }
}
