namespace VideoDownloaderApi.Abstractions.Query;

//<IQuery<IQueryResponse<IResult, IError>>, IQueryResponse<IResult, IError>>
public interface IQueryMediator : IBaseMediator
{
    public Task<TResponse> HandleAsync<TResponse>(IQuery<TResponse> query,
        CancellationToken cancellationToken = default) where TResponse : IResponse<IResult, IError>;
}
