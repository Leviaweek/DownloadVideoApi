using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record GetYoutubeVideoQuery(string Link, int? Quality) : IQuery<IResponse<IResult, IError>>
{
    public IVideoDownloader? VideoService { get; set; }
}