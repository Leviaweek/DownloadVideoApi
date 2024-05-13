using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Queries;

namespace VideoDownloaderApi.Controllers;

[ApiController]
[Route("/api/video")]
public sealed class VideoController(
    IQueryMediator<IQuery<IResponse<IResult, IError>>> queryMediator,
    ICommandMediator<ICommand<IResponse<IResult, IError>>> commandMediator)
{
    [HttpGet]
    public async Task<IResponse<IResult, IError>> FetchFormatsAsync([FromBody] FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken = default) => await queryMediator.HandleAsync(fetchFormatsQuery, cancellationToken);

    [HttpPost("download")]
    public async Task<IResponse<IResult, IError>> DownloadVideoAsync(
        [FromBody] DownloadMediaCommand downloadMediaCommand, CancellationToken cancellationToken = default) =>
        await commandMediator.HandleAsync(downloadMediaCommand, cancellationToken);
}