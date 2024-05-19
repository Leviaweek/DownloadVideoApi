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
}