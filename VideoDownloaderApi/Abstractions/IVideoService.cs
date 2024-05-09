namespace VideoDownloaderApi.Abstractions;

public interface IVideoService
{
    public IBaseMediaRepository MediaRepository { get; }
    public IVideoDownloader VideoDownloader { get; }
}