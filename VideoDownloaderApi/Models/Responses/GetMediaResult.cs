namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetMediaResult(string Message, string FilePath, string ContentType) : IResult;