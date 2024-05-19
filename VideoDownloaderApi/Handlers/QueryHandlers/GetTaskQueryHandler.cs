using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class GetTaskQueryHandler(DownloadMediaQueue downloadMediaQueue): IQueryHandler<GetTaskQuery, GetTaskResponse>
{
    public async Task<GetTaskResponse> HandleAsync(GetTaskQuery query, CancellationToken cancellationToken)
    {
        await Task.Yield();
        var task = downloadMediaQueue.GetTaskById(query.Guid);
        return task is null
            ? GetTaskResponse.TaskNotFound()
            : GetTaskResponse.Success(task.DownloadState);
    }
}