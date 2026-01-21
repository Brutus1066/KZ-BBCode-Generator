using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// XenForo modern forum BBCode generator
/// Extended BBCode with XenForo-specific features
/// </summary>
public class XenForoGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.XenForo;

    // XenForo inline spoiler
    public string InlineSpoiler(string text) => $"[ispoiler]{text}[/ispoiler]";

    // XenForo user mention
    public override string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"[USER={clean}]@{clean}[/USER]";
    }

    // XenForo media embed (supports many sites)
    public override string Video(string url) => $"[MEDIA=youtube]{ExtractYouTubeId(url)}[/MEDIA]";

    public string Media(string url, string site = "youtube")
    {
        if (site.Equals("youtube", StringComparison.OrdinalIgnoreCase))
            return $"[MEDIA=youtube]{ExtractYouTubeId(url)}[/MEDIA]";
        return $"[MEDIA={site}]{url}[/MEDIA]";
    }

    // XenForo attachments
    public string Attach(int attachmentId, string type = "full")
    {
        return $"[ATTACH={type}]{attachmentId}[/ATTACH]";
    }

    // XenForo plain text (no parsing)
    public string Plain(string text) => $"[PLAIN]{text}[/PLAIN]";

    // XenForo heading
    public override string Header(string text, HeaderLevel level)
    {
        // XenForo supports HEADING tag
        return $"[HEADING={(int)level}]{text}[/HEADING]";
    }

    // XenForo indent
    public string Indent(string text) => $"[INDENT]{text}[/INDENT]";

    // XenForo tabs
    public string Tabs(params (string title, string content)[] tabs)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("[TABS]");
        foreach (var (title, content) in tabs)
        {
            sb.AppendLine($"[TAB={title}]{content}[/TAB]");
        }
        sb.AppendLine("[/TABS]");
        return sb.ToString().TrimEnd();
    }
}
