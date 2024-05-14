using System.Collections.Concurrent;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Services;

public sealed class DownloadMediaQueue
{
    private readonly ConcurrentDictionary<Guid, DownloadTask> _downloadTasks = [];
    public Guid AddTask(MediaType mediaType,
        MediaPlatform mediaPlatform,
        string link, string videoId, int? quality = null, long? bitrate = null)
    {
        var id = Guid.NewGuid();
        var downloadTask = new DownloadTask(mediaType, mediaPlatform, link, videoId, quality, bitrate);
        _downloadTasks[id] = downloadTask;
        return id;
    }
    public DownloadTask? GetTaskById(Guid id) => _downloadTasks.GetValueOrDefault(id);

    public DownloadTask? GetFirstTask() => _downloadTasks.FirstOrDefault(x => x.Value.DownloadState is DownloadState.Waiting).Value;

    public bool RemoveTask(Guid id) => _downloadTasks.TryRemove(id, out _);
    public ConcurrentDictionary<Guid, DownloadTask> GetTasks() => _downloadTasks;
}