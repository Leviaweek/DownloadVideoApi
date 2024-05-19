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
            return FetchFormatsResponse.Success(result);
        }
        catch (HttpRequestException requestException)
        {
            return FetchFormatsResponse.ExceptionError(requestException.Message);
        }
        catch (YoutubeExplodeException youtubeExplodeException)
        {
            return FetchFormatsResponse.ExceptionError(youtubeExplodeException.Message);
        }
        catch (ArgumentException argumentException)
        {
            return FetchFormatsResponse.ExceptionError(argumentException.Message);
        }
    }

    public bool IsMatch(string link) => RegexPatterns.YoutubePattern().IsMatch(link);
}