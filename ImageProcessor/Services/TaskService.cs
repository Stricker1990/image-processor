using Azure;
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
        private static readonly string ACCOUNT_ENDPOINT = "https://localhost:8081/";
        private static readonly string AUTH_TOKEN = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private static readonly string DB_ID = "image-processor-db";
        private static readonly string CONTAINER_ID = "tasks";
        private static readonly string PARTITION_KEY = "staticPartitionKey";

        private readonly CosmosClient _cosmosClient;

        public TaskService()
        {
            _cosmosClient = new(
                accountEndpoint: ACCOUNT_ENDPOINT,
                authKeyOrResourceToken: AUTH_TOKEN
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

        public TaskEntity FinishProcessingTask(string id, string processedStoragePath)
        {
            throw new NotImplementedException();
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

        public Task<TaskEntity> StartProcessingTaskAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
