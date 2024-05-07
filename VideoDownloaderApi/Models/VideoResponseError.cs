namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record VideoResponseError: IError
{
    public required string Message { get; set; }
}