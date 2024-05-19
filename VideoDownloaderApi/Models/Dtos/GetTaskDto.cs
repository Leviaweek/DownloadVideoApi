using System.Text.Json.Serialization;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Dtos;
[Serializable]
public sealed record GetTaskDto([property: JsonPropertyName("id")]Guid Guid): IQuery<GetTaskResponse>;