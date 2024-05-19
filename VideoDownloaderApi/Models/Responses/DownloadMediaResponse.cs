using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record DownloadMediaResponse: IResponse<IResult, IError>
{
    private DownloadMediaResponse(DownloadMediaResult result) : this(true)
    {
        Result = result;
    }
    private DownloadMediaResponse(DownloadMediaError error): this(false)
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

    public static DownloadMediaResponse Success(string? id = null)
    {
        return new DownloadMediaResponse(new DownloadMediaResult(Constants.OkResponseMessage, id));
    }

    public static DownloadMediaResponse UndefinedError()
    {
        return new DownloadMediaResponse(new DownloadMediaError(Constants.UndefinedErrorMessage));
    }
}