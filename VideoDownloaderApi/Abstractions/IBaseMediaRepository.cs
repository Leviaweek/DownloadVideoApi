using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Abstractions;

public interface IBaseMediaRepository
{
    public Task<bool> SaveMediaInfoAsync(string videoId, CancellationToken cancellationToken);
    public Task<bool> UpdateMediaInfoAsync(string videoId, CancellationToken cancellationToken);
    public Task<bool> SavePhysicalMediaInfoAsync(string videoId, int quality, string format, MediaType mediaType,
        CancellationToken cancellationToken);
}