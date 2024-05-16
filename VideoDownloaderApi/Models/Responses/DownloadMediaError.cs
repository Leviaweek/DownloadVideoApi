namespace VideoDownloaderApi.Models.Responses;

public sealed record DownloadMediaError(string Message): IError;