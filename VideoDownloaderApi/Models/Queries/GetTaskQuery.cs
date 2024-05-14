using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;

namespace VideoDownloaderApi.Models.Queries;

public record GetTaskQuery(Guid Guid): IQuery<IResponse<IResult, IError>>;