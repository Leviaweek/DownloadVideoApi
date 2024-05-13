namespace VideoDownloaderApi.Abstractions.Query;

public interface IQueryHandler<in TQuery>
    where TQuery: IQuery<IResponse<IResult, IError>>
{
    public Task<IResponse<IResult, IError>> HandleAsync(TQuery query,
        CancellationToken cancellationToken);
}