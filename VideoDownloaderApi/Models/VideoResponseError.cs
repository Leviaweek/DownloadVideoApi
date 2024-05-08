namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record VideoResponseError(string Message): IError;