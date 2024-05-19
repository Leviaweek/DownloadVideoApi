using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Dtos;

[Serializable]
public sealed record DownloadMediaDto(
    [property: JsonPropertyName("link")] string Link,
    [property: JsonPropertyName("type")] MediaType Type,
    [property: JsonPropertyName("quality")]int? Quality = null,
    [property: JsonPropertyName("bitrate")]long? Bitrate = null)
    : ICommand<DownloadMediaResponse>;