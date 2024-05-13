using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Abstractions.Query;

public interface IFetchFormatsQueryHandler: IQueryHandler<FetchFormatsQuery>
{
    public bool IsMatch(string link);
}