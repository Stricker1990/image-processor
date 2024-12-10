using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string id, string fileName, string originalStoragePath);
        Task<TaskEntity> StartProcessingTaskAsync(string id);
        TaskEntity FinishProcessingTask(string id, string processedStoragePath);
        Task<TaskEntity?> GetTaskAsync(string id);
    }
}
