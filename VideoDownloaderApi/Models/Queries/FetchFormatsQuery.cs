using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record FetchFormatsQuery(string Link, int? MaxHeight = null): IQuery<IQueryResponse<IResult, IError>>;