using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
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
    public bool IsFailure => !IsSuccess;
    public JsonResult AsJsonResult() => new(this);
}