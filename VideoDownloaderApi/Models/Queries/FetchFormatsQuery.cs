using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record FetchFormatsQuery(string Link) : IQuery<FetchFormatsResponse>;