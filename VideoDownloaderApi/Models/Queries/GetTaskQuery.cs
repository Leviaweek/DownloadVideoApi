using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record GetTaskQuery(Guid Guid): IQuery<GetTaskResponse>;