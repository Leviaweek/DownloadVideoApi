using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Responses;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public sealed record GetMediaQuery(
    string Id,
    MediaPlatform Platform,
    MediaType Type,
    int? Quality,
    long? Bitrate) : IQuery<GetMediaResponse>;