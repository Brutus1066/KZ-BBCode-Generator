using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Invision Power Board (IPB) BBCode generator
/// Standard BBCode with IPB-specific features
/// </summary>
public class IPBGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.IPB;

    // IPB uses snapback for quote references
    public string Snapback(string postId) => $"[snapback]{postId}[/snapback]";

    // IPB topic link
    public string Topic(string topicId, string? text = null)
    {
        var display = string.IsNullOrWhiteSpace(text) ? topicId : text;
        return $"[topic={topicId}]{display}[/topic]";
    }

    // IPB post reference
    public string PostRef(string postId, string? text = null)
    {
        var display = string.IsNullOrWhiteSpace(text) ? postId : text;
        return $"[post={postId}]{display}[/post]";
    }

    // IPB member mention
    public override string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"[member='{clean}']{clean}[/member]";
    }

    // IPB media embed
    public override string Video(string url) => $"[media]{url}[/media]";

    // IPB background color
    public string Background(string text, string color) => $"[background={color}]{text}[/background]";
}
