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
            x => x.YoutubeVideo!.InternalVideoId == query.Id,
            cancellationToken: cancellationToken);
        if (physicalFile is null)
            return new GetMediaResponse(new GetMediaError("Media not in database"));
        if (physicalFile.Quality != query.Quality ||
            physicalFile.Bitrate != query.Bitrate ||
            physicalFile.Type != query.Type)
            return new GetMediaResponse(new GetMediaError("This media not added"));
        var (containerName, contentType) = query.Type switch
        {
            MediaType.MuxedVideo => (Constants.VideoContainerName, Constants.VideoContentType),
            MediaType.Audio => (Constants.AudioContainerName, Constants.AudioContentType),
            _ => throw new InvalidOperationException()
        };
        return new GetMediaResponse(new GetMediaResult(Constants.OkResponseMessage,
            downloader.CalculateFilePath(query.Id,
                containerName,
                query.Bitrate,
                query.Quality),
            contentType));
    }
    
    public bool IsMatch(MediaPlatform mediaPlatform) => mediaPlatform is MediaPlatform.Youtube;
}