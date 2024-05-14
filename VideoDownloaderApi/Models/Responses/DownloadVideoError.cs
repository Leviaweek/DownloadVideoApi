namespace VideoDownloaderApi.Models.Responses;

public sealed record DownloadVideoError(string Message): IError;