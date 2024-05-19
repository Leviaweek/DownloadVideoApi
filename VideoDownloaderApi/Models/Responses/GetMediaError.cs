namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetMediaError(string Message): IError;