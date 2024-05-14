using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Mediators;

public sealed class QueryMediator(IEnumerable<IFetchFormatsQueryHandler> fetchFormatsQueryHandlers, GetTaskQueryHandler getTaskQueryHandler)
    : IQueryMediator<IQuery<IResponse<IResult, IError>>>
{
    public async Task<IResponse<IResult, IError>> HandleAsync(IQuery<IResponse<IResult, IError>> query,
        CancellationToken cancellationToken = default)
    {
        return query switch
        {
            FetchFormatsQuery fetchYoutubeQuery =>
                await fetchFormatsQueryHandlers.First(x => x.IsMatch(fetchYoutubeQuery.Link))
                    .HandleAsync(fetchYoutubeQuery, cancellationToken),
            GetTaskQuery getTaskQuery => await getTaskQueryHandler.HandleAsync(getTaskQuery, cancellationToken),
            _ => throw new InvalidOperationException()
        };
    }
}