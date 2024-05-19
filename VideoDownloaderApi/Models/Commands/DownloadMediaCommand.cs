using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Commands;

[Serializable]
public sealed record DownloadMediaCommand(string Link, MediaType Type, int? Quality = null, long? Bitrate = null)
    : ICommand<DownloadMediaResponse>;