using ImageProcessor.Domain.Interfaces;

using Azure.Messaging.ServiceBus;
using System.Text.Json;

using Azure.Identity;

namespace ImageProcessor.Services
{
    public class MessageService : IMessagesService
    {
        static private readonly string TOPIC_NAME = "queue.1";
        
        private ServiceBusClient _serviceBusClient;
        private ServiceBusSender _serviceBusSender;
        public MessageService(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                _serviceBusClient = new ServiceBusClient(config.GetValue<string>("AZURE_SERVICEBUS_FULLYQUALIFIEDNAMESPACE"));
            }
            else
            {
                _serviceBusClient = new ServiceBusClient(config.GetValue<string>("AZURE_SERVICEBUS_FULLYQUALIFIEDNAMESPACE"), new DefaultAzureCredential());
            }
            
            _serviceBusSender = _serviceBusClient.CreateSender(TOPIC_NAME);
        }
        
        public async Task PublishMessage(Message messageBody)
        {
            var jsonMessage = JsonSerializer.Serialize(messageBody);

            var message = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
