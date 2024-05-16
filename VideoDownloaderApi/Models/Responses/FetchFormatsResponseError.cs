namespace VideoDownloaderApi.Models.Responses;

[Serializable]
public sealed record FetchFormatsResponseError(string Message): IError;