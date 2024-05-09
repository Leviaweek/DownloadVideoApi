using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Database.DataModels;

[Table("PhysicalYoutubeVideos", Schema = "public")]
[Serializable]
public record PhysicalYoutubeVideo
{
    [Key] public int Id { get; set; }

    public required int Quality { get; set; }
    public required MediaType Type { get; set; }

    [StringLength(maximumLength: 10, MinimumLength = 1)]
    public required string Format { get; set; }
}
    