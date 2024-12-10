using Azure;
using Microsoft.Azure.Cosmos;
using ImageProcessor.Domain.Interfaces;
using ImageProcessor.Domain.Entity;

namespace ImageProcessor.Services
{
    public class TaskService : ITaskService
    {
        private static readonly string ACCOUNT_ENDPOINT = "https://localhost:8081/";
        private static readonly string AUTH_TOKEN = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

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
                id: "image-processor-db",
                throughput: 400
            );

            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "tasks",
                partitionKeyPath: "/id"
            );

            TaskEntity task = new() {
                id = id,
                FileName = fileName,
                InitialFilePath = originalStoragePath,
                State = TaskState.Created,
            };
            await container.UpsertItemAsync(task);
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
