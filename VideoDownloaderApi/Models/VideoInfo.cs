namespace VideoDownloaderApi.Models;

public sealed record VideoInfo(string Label, string Format, int Quality) : Info(Label, Format);