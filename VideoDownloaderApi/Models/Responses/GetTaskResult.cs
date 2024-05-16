using System.Text.Json.Serialization;

namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetTaskResult(string Message, [property: JsonConverter(typeof(JsonStringEnumConverter))] DownloadState DownloadState) : IResult;