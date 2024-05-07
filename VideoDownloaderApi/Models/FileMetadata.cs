using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models;

public sealed record FileMetadata(string Type,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]AudioInfo? AudioInfo = null, 
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]VideoInfo? VideoInfo = null);