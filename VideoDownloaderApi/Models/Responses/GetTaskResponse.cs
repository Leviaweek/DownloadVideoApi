using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetTaskResponse: IResponse<IResult, IError>
{
    public GetTaskResponse(GetTaskResult getTaskResult) : this(true)
    {
        Result = getTaskResult;
    }
    public GetTaskResponse(GetTaskError getTaskError): this(false)
    {
        Error = getTaskError;
    }

    private GetTaskResponse(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
    public GetTaskResult? Result { get; }
    public GetTaskError? Error { get; }
    public bool IsSuccess { get; }
}