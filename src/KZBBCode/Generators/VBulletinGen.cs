using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// vBulletin forum BBCode generator
/// Similar to standard BBCode with some vBulletin-specific features
/// </summary>
public class VBulletinGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.VBulletin;

    // vBulletin uses VIDEO tag
    public override string Video(string url) => $"[video]{url}[/video]";

    // vBulletin highlight tag
    public string Highlight(string text) => $"[highlight]{text}[/highlight]";

    // vBulletin indent
    public string Indent(string text) => $"[indent]{text}[/indent]";

    // vBulletin thread link
    public string Thread(string threadId, string? text = null)
    {
        var display = string.IsNullOrWhiteSpace(text) ? threadId : text;
        return $"[thread={threadId}]{display}[/thread]";
    }

    // vBulletin post link
    public string Post(string postId, string? text = null)
    {
        var display = string.IsNullOrWhiteSpace(text) ? postId : text;
        return $"[post={postId}]{display}[/post]";
    }

    // vBulletin noparse (prevent BBCode parsing)
    public string NoParse(string text) => $"[noparse]{text}[/noparse]";
}
