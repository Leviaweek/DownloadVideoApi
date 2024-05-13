using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VideoDownloaderApi.Database.Models;

[Serializable]
[Table("YoutubeVideoLinks", Schema = MediaDbContext.PublicScheme)]
[EntityTypeConfiguration<YoutubeLinkVideoConfigure, YoutubeVideoLink>]
public sealed record YoutubeVideoLink
{
    [Key] public int Id { get; set; }
    public required int YoutubeVideoId { get; set; }
    [StringLength(maximumLength: 200, MinimumLength = 5)]
    public required string VideoUrl { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public YoutubeVideo? YoutubeVideo { get; set; }
}

file sealed class YoutubeLinkVideoConfigure : IEntityTypeConfiguration<YoutubeVideoLink>
{
    public void Configure(EntityTypeBuilder<YoutubeVideoLink> builder)
    {
        builder.HasOne(x => x.YoutubeVideo)
            .WithMany()
            .HasForeignKey(x => x.YoutubeVideoId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}