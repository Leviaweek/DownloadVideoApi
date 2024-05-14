using Microsoft.Extensions.Options;
using VideoDownloaderApi.Enums;
using VideoDownloaderApi.Models.Responses;
using YoutubeExplode;

namespace VideoDownloaderApi.Services;

public sealed class YoutubeVideoDownloader(
    ILogger<YoutubeVideoDownloader> logger,
    HttpClient httpClient,
    IOptions<VideoDownloaderOptions> options)
{
    private readonly string _filesPath = options.Value.CachePath;
    private const string FileNameFormat = "{0}-{1}.{2}";
    public const MediaPlatform CurrentMediaPlatform = MediaPlatform.Youtube;

    public async Task DownloadVideoAsync(string link, int quality, CancellationToken cancellationToken = default)
    {
        var client = new YoutubeClient(httpClient);
        var media = await client.Videos.GetAsync(link, cancellationToken);
        var streamInfoSet = await client.Videos.Streams.GetManifestAsync(media.Id, cancellationToken);
        var streamInfo = streamInfoSet.GetMuxedStreams()
            .FirstOrDefault(x => x.VideoQuality.MaxHeight == quality);
        if (streamInfo is null)
            throw new ArgumentException("Incorrect argument", nameof(quality));
        await client.Videos.Streams.GetAsync(streamInfo, cancellationToken);
        logger.LogInformation("Downloading video {id}:{label}.{container}", media.Id, streamInfo.VideoQuality.Label,
            streamInfo.Container.Name);
        await client.Videos.Streams.DownloadAsync(streamInfo,
            Path.Combine(_filesPath,
                string.Format(FileNameFormat, media.Id, streamInfo.VideoQuality.Label, streamInfo.Container.Name)),
            cancellationToken: cancellationToken);
    }

    public async Task DownloadAudioAsync(string link, long bitrate, CancellationToken cancellationToken = default)
    {
        var client = new YoutubeClient(httpClient);
        var media = await client.Videos.GetAsync(link, cancellationToken);
        var streamInfoSet = await client.Videos.Streams.GetManifestAsync(media.Id, cancellationToken);
        var streamInfo = streamInfoSet.GetAudioStreams()
            .FirstOrDefault(x => x.Bitrate.BitsPerSecond.Equals(bitrate));
        if (streamInfo is null)
            throw new ArgumentException("Incorrect argument", nameof(bitrate));
        await client.Videos.Streams.DownloadAsync(streamInfo,
            Path.Combine(_filesPath,
                string.Format(FileNameFormat, media.Id, streamInfo.Bitrate.BitsPerSecond, streamInfo.Container.Name)),
            cancellationToken: cancellationToken);
    }

    public async Task<VideoResponseResult> FetchFormats(string link,
        CancellationToken cancellationToken)
    {
        var client = new YoutubeClient(httpClient);
        var video = await client.Videos.GetAsync(link, cancellationToken);
        var streamInfoSet = await client.Videos.Streams.GetManifestAsync(video.Id, cancellationToken);
        var audioStreamInfo = streamInfoSet.GetAudioOnlyStreams()
            .Where(x => x.Container.Name == Constants.AudioContainerName)
            .MaxBy(x => x.Bitrate);
        var videoStreamInfo = streamInfoSet.GetMuxedStreams()
            .Where(x => x.Container.Name == Constants.VideoContainerName).ToArray();
        var fileMetadatas = videoStreamInfo.Select(x =>
                new FileMetadata(MediaType.MuxedVideo,
                    AudioInfo: new AudioInfo(Constants.AudioLabel, x.Container.Name, x.Bitrate.BitsPerSecond),
                    VideoInfo: new VideoInfo(x.VideoQuality.Label, x.Container.Name, x.VideoQuality.MaxHeight), Size: x.Size.Bytes))
            .ToList();
        if (audioStreamInfo is not null)
            fileMetadatas.Add(new FileMetadata(MediaType.Audio,
                AudioInfo: new AudioInfo(Constants.AudioLabel, audioStreamInfo.Container.Name,
                    audioStreamInfo.Bitrate.BitsPerSecond), Size: audioStreamInfo.Size.Bytes));
        return new VideoResponseResult
        {
            FileMetadatas = fileMetadatas,
            VideoId = video.Id
        };
    }
}