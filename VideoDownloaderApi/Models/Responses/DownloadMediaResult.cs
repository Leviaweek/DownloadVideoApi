using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models.Responses;

public sealed record DownloadMediaResult(
    string Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Guid = null) : IResult;