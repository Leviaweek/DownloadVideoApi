using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Abstractions.Query;

public interface IGetMediaQueryHandler: IQueryHandler<GetMediaQuery, GetMediaResponse>
{
    public bool IsMatch(MediaPlatform mediaPlatform);
}