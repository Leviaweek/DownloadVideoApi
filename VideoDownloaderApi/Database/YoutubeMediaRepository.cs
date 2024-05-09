using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Database.DataModels;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Database;

public class YoutubeMediaRepository(IDbContextFactory<YoutubeMediaDbContext> dbContextFactory)
    : IMediaRepository<YoutubeMediaRepository>
{
    public async Task<bool> SaveMediaInfoAsync(string videoId, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var video = await dbContext.YoutubeVideos.FirstOrDefaultAsync(x => x.VideoId == videoId, cancellationToken);
        if (video is not null)
            return false;
        dbContext.YoutubeVideos.Add(new YoutubeVideo
        {
            VideoId = videoId,
            CreationTime = DateTimeOffset.Now,
            LastUpdated = DateTimeOffset.Now
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> UpdateMediaInfoAsync(string videoId, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var video = await dbContext.YoutubeVideos.FirstOrDefaultAsync(x => x.VideoId == videoId, cancellationToken);
        if (video is null)
            return false;
        video.LastUpdated = DateTimeOffset.Now;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SavePhysicalMediaInfoAsync(string videoId, int quality, string format, MediaType mediaType,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var video = await dbContext.YoutubeVideos.FirstOrDefaultAsync(x => x.VideoId == videoId, cancellationToken);
        if (video is null)
            return false;
        dbContext.PhysicalYoutubeVideos.Add(new PhysicalYoutubeVideo
        {
            Format = format,
            Quality = quality,
            Type = mediaType
        });
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}