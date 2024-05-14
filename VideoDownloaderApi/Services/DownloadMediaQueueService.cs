using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Database;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Services;

public sealed class DownloadMediaQueueService(
    DownloadMediaQueue downloadMediaQueue,
    YoutubeVideoDownloader youtubeVideoDownloader,
    IDbContextFactory<MediaDbContext> dbContextFactory) : BackgroundService
{
    private const int MaxTasksCount = 5;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tasks = new List<Task>(capacity: MaxTasksCount);
        while (!stoppingToken.IsCancellationRequested)
        {
            if (tasks.Count == MaxTasksCount)
            {
                var resultTask = await Task.WhenAny(tasks);
                tasks.Remove(resultTask);
                continue;
            }
            var firstTask = downloadMediaQueue.GetFirstTask();
            if (firstTask is not null)
            {
                tasks.Add(HandleTaskAsync(firstTask, stoppingToken));
            }
            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task HandleTaskAsync(DownloadTask downloadTask, CancellationToken cancellationToken = default)
    {
        downloadTask.DownloadState = DownloadState.Downloading;

        var task = DownloadMediaAsync(downloadTask, cancellationToken);
        await task;
        if (task.IsCompleted)
            downloadTask.DownloadState = DownloadState.Completed;

        if (task.IsFaulted)
            downloadTask.DownloadState = DownloadState.Failed;
    }

    private async Task DownloadMediaAsync(DownloadTask downloadTask, CancellationToken cancellationToken = default)
    {
        var context = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        switch (downloadTask.MediaPlatform)
        {
            case MediaPlatform.Youtube:
                var physicalMedia = await context.PhysicalYoutubeMedia.FirstOrDefaultAsync(
                    x => x.YoutubeVideo!.InternalVideoId == downloadTask.VideoId &&
                         x.Bitrate == downloadTask.Bitrate &&
                         x.Quality == downloadTask.Quality,
                    cancellationToken: cancellationToken);
                ArgumentNullException.ThrowIfNull(physicalMedia);
                switch (downloadTask.MediaType)
                {
                    case MediaType.MuxedVideo:
                        ArgumentNullException.ThrowIfNull(downloadTask.Quality);
                        await youtubeVideoDownloader.DownloadVideoAsync(downloadTask.Link,
                            downloadTask.Quality.Value,
                            cancellationToken);
                        break;
                    case MediaType.Audio:
                        ArgumentNullException.ThrowIfNull(downloadTask.Bitrate);
                        await youtubeVideoDownloader.DownloadAudioAsync(downloadTask.Link,
                            downloadTask.Bitrate.Value,
                            cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                physicalMedia.IsDownloaded = true;
                await context.SaveChangesAsync(cancellationToken);
                break;
            default:
                throw new InvalidOperationException();
        }
    }
    
}