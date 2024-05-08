namespace VideoDownloaderApi.Abstractions;

public interface IQueryHandler<in TQuery, TResult, TError>
    where TQuery: IQuery<IQueryResponse<TResult, TError>>
{
    public Task<IQueryResponse<TResult, TError>> ReceiveAsync(TQuery query, IVideoDownload videoDownload,
        CancellationToken cancellationToken);
}