using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models.Commands;

public sealed record DownloadMediaCommand(string Link, MediaType Type, int? Quality = null, long? Bitrate = null)
    : ICommand<IResponse<IResult, IError>>;