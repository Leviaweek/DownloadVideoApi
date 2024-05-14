using System.Text.Json.Serialization;
using AngleSharp.Io;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

public sealed record DownloadMediaResponse: IResponse<IResult, IError>
{
    [JsonPropertyName("Result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DownloadVideoResult? Result { get; set; }

    [JsonPropertyName("Error")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DownloadVideoError? Error { get; set; }
}