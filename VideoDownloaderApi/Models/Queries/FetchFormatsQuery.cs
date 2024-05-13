using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record FetchFormatsQuery(string Link) : IQuery<IResponse<IResult, IError>>;