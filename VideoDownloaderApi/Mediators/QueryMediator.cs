using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Mediators;

public sealed class QueryMediator(
    IEnumerable<IFetchFormatsQueryHandler> fetchFormatsQueryHandlers,
    IEnumerable<IGetMediaQueryHandler> getMediaQueryHandlers,
    GetTaskQueryHandler getTaskQueryHandler)
    : IQueryMediator
{
    public async Task<TResponse> HandleAsync<TResponse>(IQuery<TResponse> query,
        CancellationToken cancellationToken = default) where TResponse: IResponse<IResult, IError>
    {
        IResponse<IResult, IError> response = query switch
        {
            FetchFormatsQuery fetchYoutubeQuery =>
                await fetchFormatsQueryHandlers.First(x => x.IsMatch(fetchYoutubeQuery.Link))
                    .HandleAsync(fetchYoutubeQuery, cancellationToken),
            GetTaskQuery getTaskQuery => await getTaskQueryHandler.HandleAsync(getTaskQuery, cancellationToken),
            GetMediaQuery getMediaQuery => await getMediaQueryHandlers.First(x => x.IsMatch(getMediaQuery.Platform))
                .HandleAsync(getMediaQuery,
                    cancellationToken),
            _ => throw new InvalidOperationException()
        };
        return (TResponse)response;
    }
}