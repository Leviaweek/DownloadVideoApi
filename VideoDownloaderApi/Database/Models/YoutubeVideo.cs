using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VideoDownloaderApi.Database.Models;

[Serializable]
[Table("YoutubeVideos", Schema = MediaDbContext.PublicScheme)]
[Index(nameof(InternalVideoId), IsUnique = true)]
public sealed record YoutubeVideo
{
    [Key] public int Id { get; set; }

    [StringLength(maximumLength: 40, MinimumLength = 5)]
    public required string InternalVideoId { get; set; }

    public required DateTimeOffset CreatedAt { get; set; }

    public required DateTimeOffset LastUpdated { get; set; }
}