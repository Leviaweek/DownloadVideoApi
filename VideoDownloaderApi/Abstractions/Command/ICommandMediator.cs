namespace VideoDownloaderApi.Abstractions.Command;

public interface ICommandMediator: IBaseMediator
{
    public Task<TResponse> HandleAsync<TResponse>(ICommand<TResponse> command,
        CancellationToken cancellationToken = default) where TResponse : IResponse<IResult, IError>;
}