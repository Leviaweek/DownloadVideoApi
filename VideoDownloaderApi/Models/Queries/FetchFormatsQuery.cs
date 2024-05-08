using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record FetchFormatsQuery(string Link): IQuery<IQueryResponse<IResult, IError>>;