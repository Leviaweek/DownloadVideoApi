using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record DownloadMediaResponse: IResponse<IResult, IError>
{
    public DownloadMediaResponse(DownloadMediaResult result) : this(true)
    {
        Result = result;
    }
    public DownloadMediaResponse(DownloadMediaError error): this(false)
    {
        Error = error;
    }

    private DownloadMediaResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
    
    [JsonPropertyName("Result")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DownloadMediaResult? Result { get; }

    [JsonPropertyName("Error")] 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DownloadMediaError? Error { get; }
    public bool IsSuccess { get; }
}