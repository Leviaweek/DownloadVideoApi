using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models.Responses;

public sealed record DownloadVideoResult(
    string Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Guid = null) : IResult;