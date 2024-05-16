using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Abstractions.Query;

public interface IGetMediaQueryHandler: IQueryHandler<GetMediaQuery>
{
    public bool IsMatch(MediaPlatform mediaPlatform);
}