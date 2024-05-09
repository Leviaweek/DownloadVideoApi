using Microsoft.EntityFrameworkCore;
using VideoDownloaderApi.Database.DataModels;

namespace VideoDownloaderApi.Database;

public class YoutubeMediaDbContext(DbContextOptions<YoutubeMediaDbContext> options): DbContext(options)
{
    public required DbSet<YoutubeVideo> YoutubeVideos { get; set; }
    public required DbSet<PhysicalYoutubeVideo> PhysicalYoutubeVideos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<YoutubeVideo>()
            .HasOne(v => v.PhysicalYoutubeVideo)
            .WithMany()
            .HasForeignKey(v => v.Id)
            .HasPrincipalKey(v => v.Id);
    }
}