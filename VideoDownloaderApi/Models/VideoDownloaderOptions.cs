using System.ComponentModel.DataAnnotations;

namespace VideoDownloaderApi.Models;

public sealed class VideoDownloaderOptions
{
    public const string OptionName = "VideoDownloaderOptions";
    [Required] public string CachePath { get; set; } = string.Empty;
}