﻿using ImageProcessor.Domain.Interfaces;

using Azure.Messaging.ServiceBus;

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
        
        public async Task PublishMessage(string message)
        {
            await _serviceBusSender.SendMessageAsync(new ServiceBusMessage(message));
        }
    }
}