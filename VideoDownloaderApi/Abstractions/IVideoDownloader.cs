using System.Text.RegularExpressions;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions;

public interface IVideoDownloader
{
    public Regex Pattern { get; }
    public bool IsMatch(string link);
    public Task DownloadAsync(GetVideoQuery getVideoQuery, CancellationToken cancellationToken);

    public Task<VideoResponseResult> FetchFormats(FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken);
}