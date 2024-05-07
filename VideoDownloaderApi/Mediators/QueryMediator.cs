using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Mediators;

public sealed class QueryMediator(IEnumerable<IVideoDownload> videoDownloadServices): IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>
{
    private readonly List<IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>> _queryHandlers = [];
    public async Task<IQueryResponse<IResult, IError>> HandleAsync(IQuery<IQueryResponse<IResult, IError>> query, CancellationToken cancellationToken = default)
    {
        switch (query)
        {
            case FetchFormatsQuery fetchFormatsQuery:
            {
                var videoDownloader =
                    videoDownloadServices.FirstOrDefault(x => x.IsMatch(fetchFormatsQuery.Link));
                var queryHandler = _queryHandlers.OfType<FetchFormatsQueryHandler>().FirstOrDefault();
                if (queryHandler is null)
                    throw new NotImplementedException();
                return await queryHandler.ReceiveAsync(fetchFormatsQuery, videoDownloader, cancellationToken);
            }
            default: throw new NotImplementedException();
        };
    }

    public void RegisterQuery(IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError> query)
    {
        _queryHandlers.Add(query);
    }
}