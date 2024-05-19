using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions.Command;

public interface IDownloadMediaCommandHandler: ICommandHandler<DownloadMediaCommand, DownloadMediaResponse>
{
    public bool IsMatch(string link);
}