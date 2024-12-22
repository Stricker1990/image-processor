namespace ImageProcessor.Domain.Interfaces
{
    public record Message
    {
        public string Id { get; set; }
        public string BlobName { get; set; }
    }
    public interface IMessagesService
    {
        Task PublishMessage(Message message);
    }
}
