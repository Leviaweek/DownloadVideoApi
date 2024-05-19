using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models.Responses;

public sealed class GetMediaResponse : IResponse<IResult, IError>
{
    public GetMediaResult? Result { get; }
    public GetMediaError? Error { get; }

    private GetMediaResponse(GetMediaResult result) : this(true)
    {
        Result = result;
    }

    private GetMediaResponse(GetMediaError error) : this(false)
    {
        Error = error;
    }

    private GetMediaResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static GetMediaResponse NotInDatabase()
    {
        return new GetMediaResponse(new GetMediaError("Media not in database"));
    }

    public static GetMediaResponse Success(string filePath, string contentType)
    {
        return new GetMediaResponse(new GetMediaResult(Constants.OkResponseMessage, filePath, contentType));
    }

    public bool IsSuccess { get; }
}