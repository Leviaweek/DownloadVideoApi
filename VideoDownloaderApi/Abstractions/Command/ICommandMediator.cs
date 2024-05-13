namespace VideoDownloaderApi.Abstractions.Command;

public interface ICommandMediator<in TCommand>: IBaseMediator
    where TCommand: ICommand<IResponse<IResult, IError>>
{
    public Task<IResponse<IResult, IError>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}