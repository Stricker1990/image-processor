using Azure;
using Azure.Data.Tables;
using ImageProcessor.Domain.Interfaces;
using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Services
{
    public record TaskTableEntity : TaskEntity, ITableEntity
    {
        public required string PartitionKey { get; set; }
        public string RowKey { get => Id; set => Id = value; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    };
    public class TaskService : ITaskService
    {
        private static readonly string TABLE_CONNECTION_STRING = "DefaultEndpointsProtocol=http;AccountName=localhost;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;TableEndpoint=http://localhost:8902/;";
        private static readonly string TABLE_NAME_TASKS = "tasks";

        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        public TaskService()
        {
            _tableServiceClient = new TableServiceClient(
                connectionString: TABLE_CONNECTION_STRING
            );
            _tableClient = _tableServiceClient.GetTableClient(tableName: TABLE_NAME_TASKS);
        }
        public async Task<TaskEntity> CreateTaskAsync(string fileName, string originalStoragePath)
        {
            
            TaskTableEntity task = new()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "",
                FileName = fileName,
                InitialFilePath = originalStoragePath,
                State = TaskState.Created
            };
            await _tableClient.UpsertEntityAsync(
                entity: task,
                mode: TableUpdateMode.Replace
            );
            return task;
        }

        public TaskEntity FinishProcessingTask(string id, string processedStoragePath)
        {
            throw new NotImplementedException();
        }

        public TaskEntity StartProcessingTask(string id)
        {
            throw new NotImplementedException();
        }
    }
}
