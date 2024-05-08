using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class FetchFormatsQueryHandler
    : IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>
{
    public async Task<IQueryResponse<IResult, IError>> ReceiveAsync(IQuery<IQueryResponse<IResult, IError>> query,
        IVideoDownload? videoDownload, CancellationToken cancellationToken)
    {
        try
        {
            if (query is not FetchFormatsQuery fetchFormatsQuery)
                throw new ArgumentException($"Incorrect argument: {nameof(query)}");

            ArgumentNullException.ThrowIfNull(videoDownload);

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