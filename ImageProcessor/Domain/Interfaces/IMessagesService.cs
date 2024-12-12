namespace ImageProcessor.Domain.Interfaces
{
    public interface IMessagesService
    {
        Task PublishMessage(string message);
    }
}
