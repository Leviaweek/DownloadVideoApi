using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoDownloaderApi.Database.DataModels;

[Table("YoutubeVideos", Schema = "public")]
public record YoutubeVideo
{
    [Key] public int Id { get; set; }

    [StringLength(maximumLength: 40, MinimumLength = 5)]
    public required string VideoId { get; set; }

    public required DateTimeOffset CreationTime { get; set; }

    public required DateTimeOffset LastUpdated { get; set; }
    public PhysicalYoutubeVideo? PhysicalYoutubeVideo { get; set; }
}