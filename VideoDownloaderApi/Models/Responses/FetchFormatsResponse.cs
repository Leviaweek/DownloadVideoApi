using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record FetchFormatsResponse : IResponse<IResult, IError>
{
    private FetchFormatsResponse(FetchFormatsResponseResult result) : this(true)
    {
        Result = result;
    }
    private FetchFormatsResponse(FetchFormatsResponseError error): this(false)
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

    public static FetchFormatsResponse ExceptionError(string message)
    {
        return new FetchFormatsResponse(new FetchFormatsResponseError(message));
    }

    public static FetchFormatsResponse Success(FetchFormatsResponseResult result)
    {
        return new FetchFormatsResponse(result);
    }
}