using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models;

public record DownloadTask(
    MediaType MediaType,
    MediaPlatform MediaPlatform,
    string Link,
    string VideoId,
    int? Quality = null,
    long? Bitrate = null)
{
    public MediaType MediaType { get; } = MediaType;
    public MediaPlatform MediaPlatform { get; } = MediaPlatform;
    public DownloadState DownloadState { get; set; } = DownloadState.Waiting;
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.Now;
    public int? Quality { get; } = Quality;
    public long? Bitrate { get; } = Bitrate;
    public string VideoId { get; } = VideoId;
    public string Link { get; } = Link;
}