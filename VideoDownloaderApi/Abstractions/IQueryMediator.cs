namespace VideoDownloaderApi.Abstractions;

//<IQuery<IQueryResponse<IResult, IError>>, IQueryResponse<IResult, IError>>
public interface IQueryMediator<TQuery, TResult, TError> : IBaseMediator
    where TQuery: IQuery<IQueryResponse<TResult, TError>>
{
    Task<IQueryResponse<TResult, TError>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
