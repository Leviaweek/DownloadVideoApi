using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions.Query;

//<IQuery<IQueryResponse<IResult, IError>>, IQueryResponse<IResult, IError>>
public interface IQueryMediator<in TQuery> : IBaseMediator
    where TQuery: IQuery<IResponse<IResult, IError>>
{
    public Task<IResponse<IResult, IError>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
