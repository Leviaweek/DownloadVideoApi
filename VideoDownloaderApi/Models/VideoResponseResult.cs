using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record VideoResponseResult: IResult
{
    [JsonPropertyName("filesMetadata")]
    public required List<FileMetadata> FileMetadatas { get; set; }
    [JsonPropertyName("videoId")]
    public required string VideoId { get; set; }
}