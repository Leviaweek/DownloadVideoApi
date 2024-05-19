using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Models.Commands;

namespace VideoDownloaderApi.Mediators;

public sealed class CommandMediator(
    IEnumerable<IDownloadMediaCommandHandler> downloadMediaCommandHandlers)
    : ICommandMediator
{
    public async Task<TResponse> HandleAsync<TResponse>(ICommand<TResponse> command,
        CancellationToken cancellationToken = default) where TResponse : IResponse<IResult, IError>
    {
        IResponse<IResult, IError> response = command switch
        {
            DownloadMediaCommand downloadMediaCommand when RegexPatterns.YoutubePattern()
                .IsMatch(downloadMediaCommand.Link) => 
                await downloadMediaCommandHandlers.First(x => x.IsMatch(downloadMediaCommand.Link))
                    .HandleAsync(downloadMediaCommand, cancellationToken),
            _ => throw new InvalidOperationException()
        };
        return (TResponse)response;
    }
}