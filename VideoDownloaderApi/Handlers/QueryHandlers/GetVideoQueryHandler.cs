using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class GetVideoQueryHandler: IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>
{
    public async Task<IQueryResponse<IResult, IError>> ReceiveAsync(IQuery<IQueryResponse<IResult, IError>> query, CancellationToken cancellationToken)
    {
        try
        {
            if (query is not GetVideoQuery getVideoQuery)
                throw new ArgumentException($"Incorrect argument: {nameof(query)}");
            ArgumentNullException.ThrowIfNull(getVideoQuery.VideoService);

            throw new NotImplementedException();
        }
        catch (HttpRequestException requestException)
        {
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError(requestException.Message)
            };
        }
        catch (ArgumentException argumentException)
        {
            return new FetchFormatsResponse
            {
                Error = new VideoResponseError(argumentException.Message)
            };
        }
    }
}