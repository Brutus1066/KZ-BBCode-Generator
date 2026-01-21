using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// MyBB forum BBCode generator
/// Standard BBCode with MyBB-specific extensions
/// </summary>
public class MyBBGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.MyBB;

    // MyBB video tag
    public override string Video(string url) => $"[video={GetVideoType(url)}]{url}[/video]";

    // MyBB PHP code highlighting
    public string PhpCode(string code) => $"[php]{code}[/php]";

    // MyBB alignment uses align tag with parameter
    public override string Align(string text, TextAlignment alignment)
    {
        var align = alignment.ToString().ToLower();
        return $"[align={align}]{text}[/align]";
    }

    private static string GetVideoType(string url)
    {
        if (url.Contains("youtube.com") || url.Contains("youtu.be"))
            return "youtube";
        if (url.Contains("vimeo.com"))
            return "vimeo";
        if (url.Contains("dailymotion.com"))
            return "dailymotion";
        return "youtube";
    }
}
