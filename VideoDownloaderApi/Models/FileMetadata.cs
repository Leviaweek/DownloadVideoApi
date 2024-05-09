using System.Text.Json.Serialization;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models;

public sealed record FileMetadata(
    [property: JsonConverter(typeof(JsonStringEnumConverter))]MediaType Type,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]AudioInfo? AudioInfo = null, 
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]VideoInfo? VideoInfo = null);