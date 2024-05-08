namespace VideoDownloaderApi.Abstractions;

//<IQuery<IQueryResponse<IResult, IError>>, IQueryResponse<IResult, IError>>
public interface IQueryMediator<in TQuery, TResult, TError> : IBaseMediator
    where TQuery: IQuery<IQueryResponse<TResult, TError>>
{
    public Task<IQueryResponse<TResult, TError>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
