using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class GetVideoQueryHandler: IQueryHandler<GetYoutubeVideoQuery>
{
    public async Task<IResponse<IResult, IError>> HandleAsync(GetYoutubeVideoQuery query, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Yield();
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