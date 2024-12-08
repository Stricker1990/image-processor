
namespace ImageProcessor.Domain.Entity
{
    public record TaskEntity
    {
        public required string Id { get; set; }
        public required string FileName { get; set; }
        public string? InitialFilePath { get; set; }
        public string? ProcessedFilePath { get; set; }
        public required TaskState State { get; set; }
    }
}
