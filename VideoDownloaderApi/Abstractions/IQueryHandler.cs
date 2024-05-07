namespace VideoDownloaderApi.Abstractions;

public interface IBaseQueryHandler;
public interface IQueryHandler<in TQuery, TResult, TError>: IBaseQuery
    where TQuery: IQuery<IQueryResponse<TResult, TError>>
{
    Task<IQueryResponse<TResult, TError>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);

    Task<IQueryResponse<TResult, TError>> ReceiveAsync(TQuery query, IVideoDownload videoDownload,
        CancellationToken cancellationToken);
}