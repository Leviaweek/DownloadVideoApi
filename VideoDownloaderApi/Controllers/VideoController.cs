using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Controllers;

[ApiController]
[Route("/api/video")]
public sealed class VideoController(IQueryMediator<IQuery<IQueryResponse<IResult, IError>>, IResult, IError> queryMediator)
{
    [HttpGet]
    public async Task<IQueryResponse<IResult, IError>> FetchFormatsAsync([FromBody] FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken = default) =>
        await queryMediator.HandleAsync(fetchFormatsQuery, cancellationToken);
}