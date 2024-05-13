namespace VideoDownloaderApi.Models;

[Serializable]
public sealed record AudioInfo(string Label, string Format, long Bitrate): Info(Label, Format);