using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class FetchFormatsQueryHandler
    : IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>
{
    private readonly IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError> _queryMediator;
    public FetchFormatsQueryHandler(IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError> queryMediator)
    {
        _queryMediator = queryMediator;
        _queryMediator.RegisterQuery(this);
    }
    public async Task<IQueryResponse<IResult, IError>> HandleAsync(IQuery<IQueryResponse<IResult, IError>> query,
        CancellationToken cancellationToken = default) => await _queryMediator.HandleAsync(query, cancellationToken);

    public async Task<IQueryResponse<IResult, IError>> ReceiveAsync(IQuery<IQueryResponse<IResult, IError>> query, IVideoDownload? videoDownload, CancellationToken cancellationToken)
    {
        if (query is not FetchFormatsQuery fetchFormatsQuery)
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError
                {
                    Message = $"Incorrect argument: {nameof(query)}"
                }
            };
        if (videoDownload is null)
        {
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError
                {
                    Message = $"Incorrect {nameof(fetchFormatsQuery.Link)}"
                }
            };
        }

        try
        {
            var result = await videoDownload.FetchFormats(fetchFormatsQuery, cancellationToken);
            return new FetchFormatsResponse
            {
                Result = result
            };
        }
        catch (HttpRequestException requestException)
        {
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError
                {
                    Message = requestException.Message
                }
            };
        }
        catch (ArgumentException argumentException)
        {
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError
                {
                    Message = argumentException.Message
                }
            };
        }
    }
}