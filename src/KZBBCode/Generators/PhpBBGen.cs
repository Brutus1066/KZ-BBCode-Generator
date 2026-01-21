using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// phpBB forum BBCode generator
/// Standard BBCode with some phpBB-specific tags
/// </summary>
public class PhpBBGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.PhpBB;

    // phpBB uses flash instead of video
    public override string Video(string url) => $"[flash=560,340]{url}[/flash]";

    // phpBB-specific attachment reference
    public string Attachment(int index = 0) => $"[attachment={index}]";

    // phpBB quote with post ID
    public string QuoteWithPostId(string text, string author, int postId)
    {
        return $"[quote=\"{author}\" post_id=\"{postId}\"]{text}[/quote]";
    }
}
