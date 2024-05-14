using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Database.Models;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Responses;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Handlers.CommandHandlers;

public sealed class DownloadYoutubeMediaCommandHandler(
    IDbContextFactory<MediaDbContext> dbContextFactory,
    YoutubeVideoDownloader youtubeVideoDownloader,
    DownloadMediaQueue downloadMediaQueue): IDownloadMediaCommandHandler
{
    public async Task<IResponse<IResult, IError>> HandleAsync(DownloadMediaCommand downloadCommand,
        CancellationToken cancellationToken)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var youtubeVideoLink = await db.YoutubeVideoLinks.Include(youtubeVideoLink => youtubeVideoLink.YoutubeVideo)
            .FirstOrDefaultAsync(x => x.VideoUrl == downloadCommand.Link,
                cancellationToken: cancellationToken);
        
        if (youtubeVideoLink is not null && youtubeVideoLink.YoutubeVideo is null)
        {
            return new DownloadMediaResponse
            {
                Error = new DownloadVideoError(Constants.UndefinedErrorMessage)
            };
        }
        
        var metadata = await youtubeVideoDownloader.FetchFormats(downloadCommand.Link, cancellationToken);
        var id = metadata.VideoId;
        
        var youtubeVideo = await db.YoutubeVideos.FirstOrDefaultAsync(x => x.InternalVideoId == id,
            cancellationToken: cancellationToken);
        if (youtubeVideo is null)
        {
            youtubeVideo = new YoutubeVideo
            {
                InternalVideoId = id,
                CreatedAt = DateTimeOffset.UtcNow,
                LastUpdated = DateTimeOffset.UtcNow
            };
            db.YoutubeVideos.Add(youtubeVideo);
            await db.SaveChangesAsync(cancellationToken);
        }

        if (youtubeVideoLink is null)
        {
            db.YoutubeVideoLinks.Add(new YoutubeVideoLink
            {
                YoutubeVideoId = youtubeVideo.Id,
                VideoUrl = downloadCommand.Link,
                CreatedAt = DateTimeOffset.UtcNow,
                YoutubeVideo = youtubeVideo
            });
        }

        var physicalYoutubeMedia =
            await db.PhysicalYoutubeMedia.OrderBy(x => x.CreatedAt).FirstOrDefaultAsync(
                x => x.YoutubeVideoId == youtubeVideo.Id && x.Quality == downloadCommand.Quality,
                cancellationToken: cancellationToken);
        var media = metadata.FileMetadatas.FirstOrDefault(x =>
        {
            return downloadCommand.Type switch
            {
                MediaType.MuxedVideo => x.Type == downloadCommand.Type &&
                                        x.VideoInfo?.Quality == downloadCommand.Quality,
                MediaType.Audio => x.Type == downloadCommand.Type && x.AudioInfo?.Bitrate == downloadCommand.Bitrate,
                _ => throw new ArgumentOutOfRangeException(nameof(downloadCommand))
            };
        });
        ArgumentNullException.ThrowIfNull(media);

        if (physicalYoutubeMedia is null)
        {
            var taskId = AddPhysicalYoutubeMedia();
            await db.SaveChangesAsync(cancellationToken);
            return new DownloadMediaResponse
            {
                Result = new DownloadVideoResult(Constants.OkResponseMessage, taskId.ToString("N"))
            };
        }

        if (physicalYoutubeMedia.Size != media.Size)
        {
            var taskId = AddPhysicalYoutubeMedia();
            physicalYoutubeMedia.IsDeleted = true;
            await db.SaveChangesAsync(cancellationToken);
            return new DownloadMediaResponse
            {
                Result = new DownloadVideoResult(Constants.OkResponseMessage, taskId.ToString("N"))
            };
        }
        return new DownloadMediaResponse
        {
            Result = new DownloadVideoResult(Constants.OkResponseMessage)
        };

        Guid AddPhysicalYoutubeMedia()
        {
            switch (downloadCommand.Type)
            {
                case MediaType.MuxedVideo:
                {
                    ArgumentNullException.ThrowIfNull(downloadCommand.Quality);
                    ArgumentNullException.ThrowIfNull(media.VideoInfo);
                    ArgumentNullException.ThrowIfNull(media.AudioInfo);
                    db.PhysicalYoutubeMedia.Add(new PhysicalYoutubeMedia
                    {
                        Type = downloadCommand.Type,
                        Quality = downloadCommand.Quality.Value,
                        YoutubeVideoId = youtubeVideo.Id,
                        Format = media.VideoInfo.Format,
                        CreatedAt = DateTimeOffset.UtcNow,
                        IsDeleted = false,
                        IsDownloaded = false,
                        Size = media.Size,
                        Bitrate = media.AudioInfo.Bitrate
                    });
                    break;
                }

                case MediaType.Audio:
                {
                    ArgumentNullException.ThrowIfNull(downloadCommand.Bitrate);
                    ArgumentNullException.ThrowIfNull(media.AudioInfo);
                    db.PhysicalYoutubeMedia.Add(new PhysicalYoutubeMedia
                    {
                        Type = downloadCommand.Type,
                        Quality = null,
                        YoutubeVideoId = youtubeVideo.Id,
                        Format = media.AudioInfo.Format,
                        CreatedAt = DateTimeOffset.UtcNow,
                        IsDeleted = false,
                        IsDownloaded = false,
                        Size = media.Size,
                        Bitrate = downloadCommand.Bitrate.Value
                    });
                    break;
                }
                default: throw new InvalidOperationException();
            }

            return downloadMediaQueue.AddTask(downloadCommand.Type,
                YoutubeVideoDownloader.CurrentMediaPlatform,
                downloadCommand.Link,
                id,
                downloadCommand.Quality,
                downloadCommand.Bitrate);
        }
    }
    public bool IsMatch(string link) => RegexPatterns.YoutubePattern().IsMatch(link);
}