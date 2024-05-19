using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetTaskResponse: IResponse<IResult, IError>
{
    private GetTaskResponse(GetTaskResult getTaskResult) : this(true)
    {
        Result = getTaskResult;
    }
    private GetTaskResponse(GetTaskError getTaskError): this(false)
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

    public static GetTaskResponse TaskNotFound()
    {
        return new GetTaskResponse(new GetTaskError("Task not found"));
    }

    public static GetTaskResponse Success(DownloadState downloadState)
    {
        return new GetTaskResponse(new GetTaskResult(Constants.OkResponseMessage,
            downloadState));
    }
}