using Microsoft.AspNetCore.Mvc;

namespace VideoDownloaderApi.Abstractions;

public interface IResponse<TResult, TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public JsonResult AsJsonResult();
}