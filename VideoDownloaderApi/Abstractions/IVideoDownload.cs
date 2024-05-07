using System.Text.RegularExpressions;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Abstractions;

public interface IVideoDownload
{
    public Regex Pattern { get; }
    public bool IsMatch(string link);
    public Task DownloadAsync(FetchFormatsQuery fetchFormatsQuery, CancellationToken cancellationToken);
    public Task<VideoResponseResult> FetchFormats(FetchFormatsQuery fetchFormatsQuery, CancellationToken cancellationToken);
}