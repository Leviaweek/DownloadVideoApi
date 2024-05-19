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
    private const string DirectoryName = "YoutubeMedia";

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
                    CalculateFilePath(media.Id,
                    streamInfo.Container.Name,
                    streamInfo.VideoQuality.MaxHeight) ,
            cancellationToken: cancellationToken);
    }

    public string CalculateFilePath(string id,
        string containerName,
        long? bitrate = null,
        int? quality = null) =>
        Path.Combine(_filesPath,
            DirectoryName,
        string.Format(FileNameFormat, id, quality ?? bitrate, containerName));

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
            CalculateFilePath(media.Id,
                streamInfo.Container.Name,
                streamInfo.Bitrate.BitsPerSecond),
            cancellationToken: cancellationToken);
    }

    public async Task<FetchFormatsResponseResult> FetchFormats(string link,
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
                    AudioInfo: new AudioInfo(Constants.AudioContainerName, x.Container.Name, x.Bitrate.BitsPerSecond),
                    VideoInfo: new VideoInfo(x.VideoQuality.Label, x.Container.Name, x.VideoQuality.MaxHeight), Size: x.Size.Bytes))
            .ToList();
        if (audioStreamInfo is not null)
            fileMetadatas.Add(new FileMetadata(MediaType.Audio,
                AudioInfo: new AudioInfo(Constants.AudioContainerName, audioStreamInfo.Container.Name,
                    audioStreamInfo.Bitrate.BitsPerSecond), Size: audioStreamInfo.Size.Bytes));
        return new FetchFormatsResponseResult
        {
            Message = Constants.OkResponseMessage,
            FileMetadatas = fileMetadatas,
            VideoId = video.Id
        };
    }
}