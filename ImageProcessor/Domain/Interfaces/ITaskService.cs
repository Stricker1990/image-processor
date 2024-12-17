using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<TaskEntity> CreateTaskAsync(string id, string fileName, string originalStoragePath);

        Task<TaskEntity> UpdateTaskAsync(TaskEntity taskEntity);
        Task<TaskEntity?> GetTaskAsync(string id);
    }
}
