namespace VideoDownloaderApi.Models;

public sealed record VideoInfo(string Label, string Format, int Max) : Info(Label, Format);