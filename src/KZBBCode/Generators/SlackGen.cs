using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Slack mrkdwn (markdown) generator
/// Uses Slack's specific markdown variant
/// </summary>
public class SlackGen : IBBCodeGen
{
    public PlatformType Platform => PlatformType.Slack;

    // Text formatting - Slack mrkdwn (note: different from standard markdown!)
    public string Bold(string text) => $"*{text}*";
    public string Italic(string text) => $"_{text}_";
    public string Underline(string text) => text; // Not supported
    public string Strikethrough(string text) => $"~{text}~";
    public string Color(string text, string color) => text; // Not supported in basic mrkdwn
    public string Size(string text, string size) => text; // Not supported
    public string Font(string text, string fontName) => text; // Not supported

    // Links
    public string Url(string url, string? text = null)
    {
        if (string.IsNullOrWhiteSpace(text))
            return $"<{url}>";
        return $"<{url}|{text}>";
    }

    public string Email(string email, string? displayText = null)
    {
        var text = string.IsNullOrWhiteSpace(displayText) ? email : displayText;
        return $"<mailto:{email}|{text}>";
    }

    public string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"@{clean}";
    }

    // Slack-specific mentions
    public string MentionById(string userId) => $"<@{userId}>";
    public string MentionChannel(string channelId) => $"<#{channelId}>";
    public string MentionUserGroup(string groupId) => $"<!subteam^{groupId}>";
    public string MentionHere() => "<!here>";
    public string MentionChannel() => "<!channel>";
    public string MentionEveryone() => "<!everyone>";

    // Media
    public string Image(string url, string? caption = null)
    {
        // Slack auto-unfurls URLs
        var result = url;
        if (!string.IsNullOrWhiteSpace(caption))
            result = $"{caption}\n{url}";
        return result;
    }

    public string Video(string url) => url;
    public string Audio(string url) => url;
    public string YouTube(string videoIdOrUrl) => ExtractYouTubeUrl(videoIdOrUrl);

    // Structure
    public string Quote(string text, string? author = null)
    {
        var lines = text.Split('\n').Select(l => $">{l}");
        var quoted = string.Join("\n", lines);
        if (!string.IsNullOrWhiteSpace(author))
            quoted = $"*{author}:*\n{quoted}";
        return quoted;
    }

    public string Code(string code, string? language = null)
    {
        if (!code.Contains('\n'))
            return $"`{code}`";
        return $"```\n{code}\n```";
    }

    public string Spoiler(string text, string? title = null)
    {
        // Slack doesn't have native spoilers
        var label = string.IsNullOrWhiteSpace(title) ? "Spoiler" : title;
        return $"[{label}]: {text}";
    }

    public string List(IEnumerable<string> items, ListType type = ListType.Bullet)
    {
        var counter = 1;
        var listItems = items.Select(i =>
        {
            if (type == ListType.Numbered)
                return $"{counter++}. {i}";
            if (type == ListType.Lettered)
                return $"{(char)('a' + counter++ - 1)}. {i}";
            return $"• {i}";
        });
        return string.Join("\n", listItems);
    }

    public string Table(string[,] cells, bool hasHeader = true)
    {
        // Slack doesn't support tables, use code block
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var colWidths = new int[cols];

        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                colWidths[c] = Math.Max(colWidths[c], cells[r, c]?.Length ?? 0);
            }
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("```");

        for (int r = 0; r < rows; r++)
        {
            sb.Append("| ");
            for (int c = 0; c < cols; c++)
            {
                var cell = cells[r, c] ?? "";
                sb.Append(cell.PadRight(colWidths[c]));
                sb.Append(" | ");
            }
            sb.AppendLine();

            if (hasHeader && r == 0)
            {
                sb.Append("|-");
                for (int c = 0; c < cols; c++)
                {
                    sb.Append(new string('-', colWidths[c]));
                    sb.Append("-|-");
                }
                sb.AppendLine();
            }
        }

        sb.AppendLine("```");
        return sb.ToString().TrimEnd();
    }

    public string Align(string text, TextAlignment alignment) => text;
    public string Header(string text, HeaderLevel level) => Bold(text);
    public string HorizontalRule() => "\n─────────────────\n";

    // Special
    public string ProgressBar(int percent, string? label = null)
    {
        var pct = Math.Clamp(percent, 0, 100);
        var filled = pct / 10;
        var bar = $"[{'█'.ToString().PadRight(filled, '█')}{'░'.ToString().PadRight(10 - filled, '░')}]";
        return $"{bar} {label ?? $"{pct}%"}";
    }

    public string Marquee(string text) => text;
    public string Hide(string text, string? buttonText = null) => Spoiler(text, buttonText);

    // Slack-specific: Date formatting
    public string Date(DateTime dateTime, string format = "{date_pretty}")
    {
        var unix = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        return $"<!date^{unix}^{format}|{dateTime:yyyy-MM-dd}>";
    }

    // Slack-specific: Custom emoji
    public string Emoji(string name) => $":{name}:";

    public string FormatText(string text, bool bold = false, bool italic = false,
        bool underline = false, bool strikethrough = false,
        string? color = null, string? size = null)
    {
        var result = text;
        if (strikethrough) result = Strikethrough(result);
        if (italic) result = Italic(result);
        if (bold) result = Bold(result);
        return result;
    }

    private static string ExtractYouTubeUrl(string videoIdOrUrl)
    {
        if (videoIdOrUrl.Contains("youtube.com") || videoIdOrUrl.Contains("youtu.be"))
            return videoIdOrUrl;
        return $"https://www.youtube.com/watch?v={videoIdOrUrl}";
    }
}
