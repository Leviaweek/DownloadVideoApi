using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record GetVideoQuery(string Link, int? Quality) : IQuery<IQueryResponse<IResult, IError>>
{
    public IVideoService? VideoService { get; set; }
}