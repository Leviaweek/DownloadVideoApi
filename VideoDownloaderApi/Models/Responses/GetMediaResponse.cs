using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models.Responses;

public sealed class GetMediaResponse : IResponse<IResult, IError>
{
    public GetMediaResult? Result { get; }
    public GetMediaError? Error { get; }

    public GetMediaResponse(GetMediaResult result) : this(true)
    {
        Result = result;
    }

    public GetMediaResponse(GetMediaError error) : this(false)
    {
        Error = error;
    }

    private GetMediaResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public JsonResult AsJsonResult() => new(this);

    public PhysicalFileResult AsPhysicalFileResult()
    {
        if (IsFailure)
            throw new InvalidCastException();
        
        ArgumentNullException.ThrowIfNull(Result);
        
        var contentType = Result.Type switch
        {
            MediaType.MuxedVideo => "video/mp4",
            MediaType.Audio => "audio/mp4",
            _ => throw new InvalidOperationException()
        };
        return new PhysicalFileResult(Result.FilePath, contentType);
    }

}

[Serializable]
public sealed record GetMediaResult(string Message, string FilePath, MediaType Type) : IResult;

[Serializable]
public sealed record GetMediaError(string Message): IError;