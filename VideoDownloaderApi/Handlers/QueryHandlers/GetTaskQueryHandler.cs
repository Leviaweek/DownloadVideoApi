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
        return task is null
            ? new GetTaskResponse(new GetTaskError("Undefined task"))
            : new GetTaskResponse(new GetTaskResult(Constants.OkResponseMessage,
                task.DownloadState));
    }
}