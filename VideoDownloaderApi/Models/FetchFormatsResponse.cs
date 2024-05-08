using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record FetchFormatsResponse: IQueryResponse<IResult, IError>
{
    [JsonPropertyName("Result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseResult? Result { get; set; }

    [JsonPropertyName("Error")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseError? Error;
}