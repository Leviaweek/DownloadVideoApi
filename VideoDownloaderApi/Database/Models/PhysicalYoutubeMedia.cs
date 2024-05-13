using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Database.Models;

[Serializable]
[Table("PhysicalYoutubeMedia", Schema = MediaDbContext.PublicScheme)]
[EntityTypeConfiguration<PhysicalYoutubeVideoConfigure, PhysicalYoutubeMedia>]
public sealed record PhysicalYoutubeMedia
{
    [Key] public int Id { get; set; }
    public required int YoutubeVideoId { get; set; }
    public required int? Quality { get; set; }
    public required long? Bitrate { get; set; }
    public required MediaType Type { get; set; }

    [StringLength(maximumLength: 10, MinimumLength = 1)]
    public required string Format { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public required bool IsDeleted { get; set; }
    public required bool IsDownloaded { get; set; }
    public required long Size { get; set; }
    public YoutubeVideo? YoutubeVideo { get; set; }
}

file sealed class PhysicalYoutubeVideoConfigure : IEntityTypeConfiguration<PhysicalYoutubeMedia>
{
    public void Configure(EntityTypeBuilder<PhysicalYoutubeMedia> builder)
    {
        builder.HasOne(x => x.YoutubeVideo)
            .WithMany()
            .HasForeignKey(x => x.YoutubeVideoId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}