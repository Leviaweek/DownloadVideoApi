using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions.Query;

public interface IFetchFormatsQueryHandler: IQueryHandler<FetchFormatsQuery, FetchFormatsResponse>
{
    public bool IsMatch(string link);
}