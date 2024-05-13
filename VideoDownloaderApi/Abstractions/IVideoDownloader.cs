using System.Text.RegularExpressions;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions;

public interface IVideoDownloader
{
    public bool IsMatch(string link);
    public Task DownloadVideoAsync(string link, int quality, CancellationToken cancellationToken);

    public Task<VideoResponseResult> FetchFormats(string link,
        CancellationToken cancellationToken);
}