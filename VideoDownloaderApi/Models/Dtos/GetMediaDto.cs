using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Dtos;
[Serializable]
public sealed record GetMediaDto(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("platform")]MediaPlatform Platform,
    [property: JsonPropertyName("type")]MediaType Type,
    [property: JsonPropertyName("quality")]int? Quality = null,
    [property: JsonPropertyName("bitrate")]long? Bitrate = null) : IQuery<GetMediaResponse>;