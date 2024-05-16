using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions;

public interface IVideoDownloader
{
    public bool IsMatch(string link);
    public Task DownloadVideoAsync(string link, int quality, CancellationToken cancellationToken);

    public Task<FetchFormatsResponseResult> FetchFormats(string link,
        CancellationToken cancellationToken);
}