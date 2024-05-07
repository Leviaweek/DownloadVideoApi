namespace VideoDownloaderApi.Models;

public sealed record AudioInfo(string Label, string Format, double Bitrate): Info(Label, Format);