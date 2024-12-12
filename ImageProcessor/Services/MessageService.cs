using ImageProcessor.Domain.Interfaces;

using Azure.Messaging.ServiceBus;

namespace ImageProcessor.Services
{
    public class MessageService : IMessagesService
    {
        static private readonly string CONNECTION_STRING = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
        static private readonly string TOPIC_NAME = "queue.1";
        
        private ServiceBusClient _serviceBusClient;
        private ServiceBusSender _serviceBusSender;
        public MessageService()
        {
            _serviceBusClient = new ServiceBusClient(CONNECTION_STRING);
            _serviceBusSender = _serviceBusClient.CreateSender(TOPIC_NAME);
        }
        
        public async Task PublishMessage(string message)
        {
            await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}
