using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Mediators;

public sealed class QueryMediator(
    IEnumerable<IVideoService> videoDownloadServices,
    IEnumerable<IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>> queryHandlers)
    : IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>
{
    public async Task<IQueryResponse<IResult, IError>> HandleAsync(IQuery<IQueryResponse<IResult, IError>> query,
        CancellationToken cancellationToken = default)
    {
        switch (query)
        {
            case FetchFormatsQuery fetchFormatsQuery:
            {
                var videoDownloader =
                    videoDownloadServices.FirstOrDefault(x => x.VideoDownloader.IsMatch(fetchFormatsQuery.Link));
                var queryHandler = queryHandlers.OfType<FetchFormatsQueryHandler>().FirstOrDefault();
                if (queryHandler is null)
                    throw new InvalidOperationException();
                fetchFormatsQuery.VideoService = videoDownloader;
                return await queryHandler.ReceiveAsync(fetchFormatsQuery, cancellationToken);
            }
            default: throw new InvalidOperationException();
        }
    }
}