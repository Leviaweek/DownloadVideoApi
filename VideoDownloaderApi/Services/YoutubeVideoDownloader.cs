using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using VideoDownloaderApi.Abstractions;
using VideoDownloaderApi.Models.Queries;
using YoutubeExplode;

namespace VideoDownloaderApi.Services;

public sealed partial class YoutubeVideoDownloader(
    ILogger<YoutubeVideoDownloader> logger,
    HttpClient httpClient,
    IOptions<VideoDownloaderOptions> options) : IVideoDownload
{
    public Regex Pattern { get; } = YoutubePattern();
    private readonly string _filesPath = options.Value.CachePath;
    private const string FileNameFormat = "{0}-{1}.{2}";

    public async Task DownloadAsync(GetVideoQuery getVideoQuery, CancellationToken cancellationToken = default)
    {
        if (getVideoQuery.Quality is null)
            throw new ArgumentException("Incorrect argument", nameof(getVideoQuery));
        var client = new YoutubeClient(httpClient);
        var video = await client.Videos.GetAsync(getVideoQuery.Link, cancellationToken);
        var streamInfoSet = await client.Videos.Streams.GetManifestAsync(video.Id, cancellationToken);
        var streamInfo = streamInfoSet.GetMuxedStreams()
            .FirstOrDefault(x => x.VideoQuality.MaxHeight == getVideoQuery.Quality);
        if (streamInfo is null)
            throw new ArgumentException("Incorrect argument", nameof(getVideoQuery));
        await client.Videos.Streams.GetAsync(streamInfo, cancellationToken);
        logger.LogInformation("Downloading video {id}:{label}.{container}", video.Id, streamInfo.VideoQuality.Label,
            streamInfo.Container.Name);
        await client.Videos.Streams.DownloadAsync(streamInfo,
            Path.Combine(_filesPath,
                string.Format(FileNameFormat, video.Id, streamInfo.VideoQuality.Label, streamInfo.Container.Name)),
            cancellationToken: cancellationToken);
    }

    public async Task<VideoResponseResult> FetchFormats(FetchFormatsQuery fetchFormatsQuery,
        CancellationToken cancellationToken)
    {
        var client = new YoutubeClient(httpClient);
        var video = await client.Videos.GetAsync(fetchFormatsQuery.Link, cancellationToken);
        var streamInfoSet = await client.Videos.Streams.GetManifestAsync(video.Id, cancellationToken);
        var audioStreamInfo = streamInfoSet.GetAudioOnlyStreams()
            .Where(x => x.Container.Name == Constants.AudioContainerName)
            .MaxBy(x => x.Bitrate);
        var videoStreamInfo = streamInfoSet.GetMuxedStreams()
            .Where(x => x.Container.Name == Constants.VideoContainerName).ToArray();
        var fileMetadatas = videoStreamInfo.Select(x =>
                new FileMetadata(Constants.MetadatasVideoType,
                    AudioInfo: new AudioInfo(x.AudioCodec, x.Container.Name, x.Bitrate.BitsPerSecond),
                    VideoInfo: new VideoInfo(x.VideoQuality.Label, x.Container.Name, x.VideoQuality.MaxHeight)))
            .ToList();
        if (audioStreamInfo is not null)
            fileMetadatas.Add(new FileMetadata(Constants.MetadatasAudioType,
                AudioInfo: new AudioInfo(audioStreamInfo.AudioCodec, audioStreamInfo.Container.Name,
                    audioStreamInfo.Bitrate.KiloBitsPerSecond)));
        return new VideoResponseResult
        {
            FileMetadatas = fileMetadatas,
            VideoId = video.Id
        };
    }

    public bool IsMatch(string link) => Pattern.IsMatch(link);

    [GeneratedRegex(@"https?:\/\/(www.)?youtu\.?be(\.com?)?\/\w+")]
    private static partial Regex YoutubePattern();
}