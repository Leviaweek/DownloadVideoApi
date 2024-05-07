using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record FetchFormatsResponse: IQueryResponse<IResult, IError>
{
    [JsonPropertyName("VideoResponseResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseResult? Result { get; set; }

    [JsonPropertyName("VideoResponseError")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseError? Error;
}