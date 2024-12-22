using ImageProcessor.Domain.Interfaces;

using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace ImageProcessor.Services
{
    public class MessageService : IMessagesService
    {
        static private readonly string TOPIC_NAME = "queue.1";
        
        private ServiceBusClient _serviceBusClient;
        private ServiceBusSender _serviceBusSender;
        public MessageService(IConfiguration config)
        {
            _serviceBusClient = new ServiceBusClient(config.GetConnectionString("BusService"));
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
