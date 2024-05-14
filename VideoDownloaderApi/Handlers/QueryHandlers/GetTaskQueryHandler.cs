using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public class GetTaskQueryHandler(DownloadMediaQueue downloadMediaQueue): IQueryHandler<GetTaskQuery>
{
    public async Task<IResponse<IResult, IError>> HandleAsync(GetTaskQuery query, CancellationToken cancellationToken)
    {
        await Task.Yield();
        var task = downloadMediaQueue.GetTaskById(query.Guid);
        if (task is null)
            return new GetTaskResponse
            {
                Error = new GetTaskError("Undefined task")
            };
        return new GetTaskResponse()
        {
            Result = new GetTaskResult(Constants.OkResponseMessage, task.DownloadState)
        };
    }
}