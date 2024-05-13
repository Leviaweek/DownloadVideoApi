using VideoDownloaderApi.Models.Commands;

namespace VideoDownloaderApi.Abstractions.Command;

public interface IDownloadMediaCommandHandler: ICommandHandler<DownloadMediaCommand>
{
    public bool IsMatch(string link);
}