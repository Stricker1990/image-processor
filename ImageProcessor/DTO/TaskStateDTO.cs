using ImageProcessor.Domain.Entity;
using System.Text.Json.Serialization;

namespace ImageProcessor.DTO
{
    public record TaskStateDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskState State { get; set; }
        public string? FileURL { get; set; }
        public required string FileName { get; set; }
    }
}
