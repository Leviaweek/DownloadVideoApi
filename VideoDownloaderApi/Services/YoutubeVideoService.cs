using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Database;

namespace VideoDownloaderApi.Services;

public sealed class YoutubeVideoService(
    IVideoDownloader youtubeVideoDownloader,
    IMediaRepository<YoutubeMediaRepository> mediaRepository)
    : IVideoService
{
    public IBaseMediaRepository MediaRepository { get; } = mediaRepository;
    public IVideoDownloader VideoDownloader { get; } = youtubeVideoDownloader;
}