namespace VideoDownloaderApi.Abstractions.Command;
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand<IResponse<IResult, IError>>
{
    public Task<IResponse<IResult, IError>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}