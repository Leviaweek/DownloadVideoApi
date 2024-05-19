using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public sealed class GetMediaYoutubeQueryHandler(
    IDbContextFactory<MediaDbContext> factory,
    YoutubeVideoDownloader downloader) : IGetMediaQueryHandler
{
    public async Task<GetMediaResponse> HandleAsync(GetMediaQuery query, CancellationToken cancellationToken)
    {
        await using var context = await factory.CreateDbContextAsync(cancellationToken);
        var physicalFile = await context.PhysicalYoutubeMedia.FirstOrDefaultAsync(
            x => x.YoutubeVideo!.InternalVideoId == query.Id &&
                 x.Quality == query.Quality &&
                 x.Bitrate == query.Bitrate &&
                 x.Type == query.Type,
            cancellationToken: cancellationToken);
        if (physicalFile is null)
            GetMediaResponse.NotInDatabase();
        var (containerName, contentType) = query.Type switch
        {
            MediaType.MuxedVideo => (Constants.VideoContainerName, Constants.VideoContentType),
            MediaType.Audio => (Constants.AudioContainerName, Constants.AudioContentType),
            _ => throw new InvalidOperationException()
        };
        return GetMediaResponse.Success(downloader.CalculateFilePath(query.Id,
                containerName,
                query.Bitrate,
                query.Quality),
            contentType);
    }
    
    public bool IsMatch(MediaPlatform mediaPlatform) => mediaPlatform is MediaPlatform.Youtube;
}