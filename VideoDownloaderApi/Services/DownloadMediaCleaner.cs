namespace VideoDownloaderApi.Services;

public sealed class DownloadMediaCleaner(ILogger<DownloadMediaCleaner> logger, DownloadMediaQueue downloadMediaQueue): BackgroundService
{
    private DateTimeOffset _lastClearTime;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if ((DateTimeOffset.UtcNow - _lastClearTime).TotalDays >= 1)
            {
                ClearOldTasks();
                logger.LogInformation("Cleared old tasks");
            }

            await Task.Delay(1000 * 60, stoppingToken);
        }
    }

    private void ClearOldTasks()
    {
        var now = DateTimeOffset.UtcNow;
        var tasks = downloadMediaQueue.GetTasks();
        foreach (var task in tasks.Where(task =>
                     task.Value.DownloadState is DownloadState.Completed or DownloadState.Failed &&
                     (now - task.Value.CreatedAt).TotalDays >= 1))
        {
            downloadMediaQueue.RemoveTask(task.Key);
        }
        _lastClearTime = now;
    }
}