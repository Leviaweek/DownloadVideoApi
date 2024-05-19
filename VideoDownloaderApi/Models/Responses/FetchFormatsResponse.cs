using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record FetchFormatsResponse : IResponse<IResult, IError>
{
    public FetchFormatsResponse(FetchFormatsResponseResult result) : this(true)
    {
        Result = result;
    }
    public FetchFormatsResponse(FetchFormatsResponseError error): this(false)
    {
        Error = error;
    }
    private FetchFormatsResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
    [JsonPropertyName("Result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FetchFormatsResponseResult? Result { get; }
    
    [JsonPropertyName("Error")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FetchFormatsResponseError? Error { get; }

    public bool IsSuccess { get; }
}