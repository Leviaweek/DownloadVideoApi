using Microsoft.EntityFrameworkCore;
using Npgsql;
using VideoDownloaderApi.Database.Models;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Database;

[Serializable]
public sealed class MediaDbContext(DbContextOptions<MediaDbContext> options): DbContext(options)
{
    public const string PublicScheme = "public";
    public required DbSet<YoutubeVideo> YoutubeVideos { get; set; }
    public required DbSet<PhysicalYoutubeMedia> PhysicalYoutubeMedia { get; set; }
    public required DbSet<YoutubeVideoLink> YoutubeVideoLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.HasPostgresEnum<MediaType>(schema: PublicScheme);
}