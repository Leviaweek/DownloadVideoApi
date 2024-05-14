using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record FetchFormatsResponse: IResponse<IResult, IError>
{
    [JsonPropertyName("Result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseResult? Result { get; set; }
    
    [JsonPropertyName("Error")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public VideoResponseError? Error { get; set; }
}