using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VideoDownloaderApi.Abstractions.Command;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Commands;
using VideoDownloaderApi.Models.Dtos;
using VideoDownloaderApi.Models.Queries;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Controllers;


[ApiController]
[Route("/api/video")]
public sealed class VideoController(
    IQueryMediator queryMediator,
    ICommandMediator commandMediator)
{
    [HttpGet("fetchFormats")]
    public async Task<Results<Ok<FetchFormatsResponseResult>, NotFound<FetchFormatsResponseError>>> FetchFormatsAsync(
        [FromBody] FetchFormatsDto fetchFormatsDto,
        CancellationToken cancellationToken = default)
    {
        var fetchFormatsQuery = new FetchFormatsQuery(fetchFormatsDto.Link);
        var response = await queryMediator.HandleAsync(fetchFormatsQuery, cancellationToken);
        return response.IsSuccess ? TypedResults.Ok(response.Result) : TypedResults.NotFound(response.Error);
    }

    [HttpPost("download")]
    public async Task<Results<Ok<DownloadMediaResult>, NotFound<DownloadMediaError>>> DownloadVideoAsync(
        [FromBody] DownloadMediaDto downloadMediaDto, CancellationToken cancellationToken = default)
    {
        var downloadMediaCommand = new DownloadMediaCommand(
            Link: downloadMediaDto.Link,
            Type: downloadMediaDto.Type,
            Quality: downloadMediaDto.Quality,
            Bitrate: downloadMediaDto.Bitrate);
        var response = await commandMediator.HandleAsync(downloadMediaCommand, cancellationToken);
        return response.IsSuccess ? TypedResults.Ok(response.Result) : TypedResults.NotFound(response.Error);

    }

    [HttpGet("task")]
    public async Task<Results<Ok<GetTaskResult>, NotFound<GetTaskError>>> GetTaskAsync([FromBody] GetTaskDto getTaskDto,
        CancellationToken cancellationToken = default)
    {
        var getTaskQuery = new GetTaskQuery(getTaskDto.Guid);
        var response = await queryMediator.HandleAsync(getTaskQuery, cancellationToken);
        return response.IsSuccess ? TypedResults.Ok(response.Result) : TypedResults.NotFound(response.Error);
    }

    [HttpGet("media")]
    public async Task<Results<PhysicalFileHttpResult, NotFound<GetMediaError>>> GetMediaAsync([FromBody] GetMediaDto getMediaDto,
        CancellationToken cancellationToken = default)

    {
        var getMediaQuery = new GetMediaQuery(
            Id: getMediaDto.Id,
            Platform: getMediaDto.Platform,
            Type: getMediaDto.Type,
            Quality: getMediaDto.Quality,
            Bitrate: getMediaDto.Bitrate);
        var response = await queryMediator.HandleAsync(getMediaQuery, cancellationToken);
        return response.IsSuccess
            ? TypedResults.PhysicalFile(response.Result?.FilePath ?? throw new InvalidOperationException(),
                response.Result.ContentType)
            : TypedResults.NotFound(response.Error);
    }
}