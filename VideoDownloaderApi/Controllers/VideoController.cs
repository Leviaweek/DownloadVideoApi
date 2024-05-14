using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Services;

namespace VideoDownloaderApi.Controllers;

[ApiController]
[Route("/api/video")]
public sealed class VideoController(
    IQueryMediator<IQuery<IResponse<IResult, IError>>> queryMediator,
    ICommandMediator<ICommand<IResponse<IResult, IError>>> commandMediator,
    DownloadMediaQueue downloadMediaQueue)
{
    [HttpGet("fetchFormats")]
    public async Task<IResponse<IResult, IError>> FetchFormatsAsync([FromBody] FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken = default) => await queryMediator.HandleAsync(fetchFormatsQuery, cancellationToken);

    [HttpPost("download")]
    public async Task<IResponse<IResult, IError>> DownloadVideoAsync(
        [FromBody] DownloadMediaCommand downloadMediaCommand, CancellationToken cancellationToken = default) =>
        await commandMediator.HandleAsync(downloadMediaCommand, cancellationToken);

    [HttpGet("task")]
    public async Task<IResponse<IResult, IError>> GetTaskAsync([FromBody] GetTaskQuery getTaskQuery) =>
        await queryMediator.HandleAsync(getTaskQuery);
}