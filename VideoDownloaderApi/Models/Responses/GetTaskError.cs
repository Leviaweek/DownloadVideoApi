namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record GetTaskError(string Message): IError;