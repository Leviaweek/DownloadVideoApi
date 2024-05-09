namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record VideoResponseError(string Message): IError;