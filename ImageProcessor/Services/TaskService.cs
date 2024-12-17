using Microsoft.Azure.Cosmos;
using ImageProcessor.Domain.Interfaces;
using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Services
{
    public record TaskEntityCosmosDB : TaskEntity
    {
        public required string partitionKey {get; set;}
    }
    public class TaskService : ITaskService
    {
        private static readonly string DB_ID = "image-processor-db";
        private static readonly string CONTAINER_ID = "tasks";
        private static readonly string PARTITION_KEY = "staticPartitionKey";

        private readonly CosmosClient _cosmosClient;

        public TaskService(IConfiguration config)
        {
            _cosmosClient = new(
                accountEndpoint: config.GetValue<string>("CosmosDB:EndPoint"),
                authKeyOrResourceToken: config.GetValue<string>("CosmosDB:AuthToken")
            );
        }
        public async Task<TaskEntity> CreateTaskAsync(string id, string fileName, string originalStoragePath)
        {
            Database database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(
                id: DB_ID,
                throughput: 400
            );

            Container container = await database.CreateContainerIfNotExistsAsync(
                id: CONTAINER_ID,
                partitionKeyPath: "/partitionKey"
            );

            TaskEntityCosmosDB task = new() {
                id = id,
                FileName = fileName,
                InitialFilePath = originalStoragePath,
                State = TaskState.Created,
                partitionKey = PARTITION_KEY
            };
            await container.UpsertItemAsync(task);
            return task;
        }

        public async Task<TaskEntity?> GetTaskAsync(string id)
        {
            var container = _cosmosClient.GetDatabase(DB_ID)?.GetContainer(CONTAINER_ID);
            if (container == null)
            {
                throw new Exception("Container doesn't exists");
            }
            return await container.ReadItemAsync<TaskEntity>(id, new PartitionKey(PARTITION_KEY));
        }

        public async Task<TaskEntity> UpdateTaskAsync(TaskEntity taskEntity)
        {
            var container = _cosmosClient.GetDatabase(DB_ID)?.GetContainer(CONTAINER_ID);
            TaskEntityCosmosDB task = new()
            {
                id = taskEntity.id,
                FileName = taskEntity.FileName,
                InitialFilePath = taskEntity.InitialFilePath,
                ProcessedFilePath = taskEntity.ProcessedFilePath,
                State = taskEntity.State,
                partitionKey = PARTITION_KEY
            };
            return await container?.UpsertItemAsync(task);
        }
    }
}
