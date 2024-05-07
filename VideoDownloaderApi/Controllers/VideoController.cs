using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Handlers.QueryHandlers;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Controllers;

[ApiController]
[Route("/api/video")]
public sealed class VideoController(IEnumerable<IQueryHandler<IQuery<IQueryResponse<IResult, IError>>, IResult, IError>> handlers)
{
    [HttpGet]
    public async Task<IQueryResponse<IResult, IError>> FetchFormatsAsync([FromBody] FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken = default)
    {
        var handler = handlers.OfType<FetchFormatsQueryHandler>().FirstOrDefault();
        if (handler is null)
            throw new NotImplementedException();
        return await handler.HandleAsync(fetchFormatsQuery, cancellationToken);
    }
}