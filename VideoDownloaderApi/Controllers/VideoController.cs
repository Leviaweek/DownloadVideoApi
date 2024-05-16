using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Controllers;

[ApiController]
[Route("/api/video")]
public sealed class VideoController(
    IQueryMediator<IQuery<IResponse<IResult, IError>>> queryMediator,
    ICommandMediator<ICommand<IResponse<IResult, IError>>> commandMediator)
{
    [HttpGet("fetchFormats")]
    public async Task<IActionResult> FetchFormatsAsync([FromBody] FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken = default) => (await queryMediator.HandleAsync(fetchFormatsQuery, cancellationToken)).AsJsonResult();

    [HttpPost("download")]
    public async Task<IActionResult> DownloadVideoAsync(
        [FromBody] DownloadMediaCommand downloadMediaCommand, CancellationToken cancellationToken = default) =>
        (await commandMediator.HandleAsync(downloadMediaCommand, cancellationToken)).AsJsonResult();

    [HttpGet("task")]
    public async Task<IActionResult> GetTaskAsync([FromBody] GetTaskQuery getTaskQuery,
        CancellationToken cancellationToken = default) =>
        (await queryMediator.HandleAsync(getTaskQuery, cancellationToken)).AsJsonResult();

    [HttpGet("media")]
    public async Task<IActionResult> GetMediaAsync([FromBody] GetMediaQuery getMediaQuery,
        CancellationToken cancellationToken = default)
    {
        var response = await queryMediator.HandleAsync(getMediaQuery, cancellationToken);
        if (response is not GetMediaResponse getMediaResponse) throw new InvalidOperationException();

        if (getMediaResponse.IsFailure) return getMediaResponse.AsJsonResult();
        
        return getMediaResponse.AsPhysicalFileResult();
    }
}