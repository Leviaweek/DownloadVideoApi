using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Models.Commands;

namespace VideoDownloaderApi.Mediators;

public sealed class CommandMediator(
    IEnumerable<IDownloadMediaCommandHandler> downloadMediaCommandHandlers)
    : ICommandMediator<ICommand<IResponse<IResult, IError>>>
{
    public async Task<IResponse<IResult, IError>> HandleAsync(ICommand<IResponse<IResult, IError>> command, CancellationToken cancellationToken = default)
    {
        return command switch
        {
            DownloadMediaCommand downloadMediaCommand when RegexPatterns.YoutubePattern()
                .IsMatch(downloadMediaCommand.Link) => 
                await downloadMediaCommandHandlers.First(x => x.IsMatch(downloadMediaCommand.Link))
                    .HandleAsync(downloadMediaCommand, cancellationToken),
            _ => throw new InvalidOperationException()
        };
    }
}