using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;
using YoutubeExplode.Exceptions;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class FetchYoutubeFormatsQueryHandler(YoutubeVideoDownloader youtubeVideoDownloader): IFetchFormatsQueryHandler
{
    public async Task<FetchFormatsResponse> HandleAsync(FetchFormatsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await youtubeVideoDownloader.FetchFormats(query.Link, cancellationToken);
            return new FetchFormatsResponse(result);
        }
        catch (HttpRequestException requestException)
        {
            return new FetchFormatsResponse(new FetchFormatsResponseError(requestException.Message));
        }
        catch (YoutubeExplodeException youtubeExplodeException)
        {
            return new FetchFormatsResponse(new FetchFormatsResponseError(youtubeExplodeException.Message));
        }
        catch (ArgumentException argumentException)
        {
            return new FetchFormatsResponse(new FetchFormatsResponseError(argumentException.Message));
        }
    }

    public bool IsMatch(string link) => RegexPatterns.YoutubePattern().IsMatch(link);
}