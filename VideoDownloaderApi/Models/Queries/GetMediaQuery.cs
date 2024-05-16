using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Abstractions.Query;
using VideoDownloaderApi.Enums;

namespace VideoDownloaderApi.Models.Queries;

[Serializable]
public record GetMediaQuery(string Id, MediaPlatform Platform, MediaType Type, int? Quality, long? Bitrate): IQuery<IResponse<IResult, IError>>;