using System.Text.RegularExpressions;

namespace VideoDownloaderApi;

public static partial class RegexPatterns
{
    [GeneratedRegex(@"https?:\/\/(www.)?youtu\.?be(\.com?)?\/\w+")]
    public static partial Regex YoutubePattern();
}