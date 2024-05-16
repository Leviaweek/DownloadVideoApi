using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Handlers.QueryHandlers;

public class GetMediaYoutubeQueryHandler(IDbContextFactory<MediaDbContext> factory, YoutubeVideoDownloader downloader): IGetMediaQueryHandler
{
    public async Task<IResponse<IResult, IError>> HandleAsync(GetMediaQuery query, CancellationToken cancellationToken)
    {
        await using var context = await factory.CreateDbContextAsync(cancellationToken);
        var physicalFile = await context.PhysicalYoutubeMedia.FirstOrDefaultAsync(
            x => x.YoutubeVideo!.InternalVideoId == query.Id,
            cancellationToken: cancellationToken);
        if (physicalFile is null)
            return new DownloadMediaResponse(new DownloadMediaError("Media not in database"));
        if (physicalFile.Quality != query.Quality ||
            physicalFile.Bitrate != query.Bitrate ||
            physicalFile.Type != query.Type)
            return new DownloadMediaResponse(new DownloadMediaError("This media not added"));
        var containterName = query.Type switch
        {
            MediaType.MuxedVideo => Constants.VideoContainerName,
            MediaType.Audio => Constants.AudioContainerName,
            _ => throw new InvalidOperationException()
        };
        return new GetMediaResponse(new GetMediaResult(Constants.OkResponseMessage,
            downloader.CalculateFilePath(query.Id,
                containterName,
                query.Bitrate,
                query.Quality),
            query.Type));
    }

    public bool IsMatch(MediaPlatform mediaPlatform) => mediaPlatform is MediaPlatform.Youtube;
}