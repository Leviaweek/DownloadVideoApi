using System.ComponentModel;
using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions;

namespace VideoDownloaderApi.Models.Responses;

public class GetTaskResponse: IResponse<IResult, IError>
{
    public GetTaskResult? Result { get; set; }
    public GetTaskError? Error { get; set; }
}

public record GetTaskResult(string Message, [property: JsonConverter(typeof(JsonStringEnumConverter))] DownloadState DownloadState) : IResult;

public record GetTaskError(string Message): IError;