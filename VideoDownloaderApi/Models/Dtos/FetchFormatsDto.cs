using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models.Dtos;

[Serializable]
public record FetchFormatsDto([property: JsonPropertyName("link")]string Link);