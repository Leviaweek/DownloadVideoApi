namespace VideoDownloaderApi.Abstractions;

public interface IResponse<TResult, TError>
{
    public bool IsSuccess { get; }
}