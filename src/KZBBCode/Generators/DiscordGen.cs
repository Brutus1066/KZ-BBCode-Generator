using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Discord markdown generator
/// Uses Discord-flavored markdown
/// </summary>
public class DiscordGen : IBBCodeGen
{
    public PlatformType Platform => PlatformType.Discord;

    // Text formatting - Discord Markdown
    public string Bold(string text) => $"**{text}**";
    public string Italic(string text) => $"*{text}*";
    public string Underline(string text) => $"__{text}__";
    public string Strikethrough(string text) => $"~~{text}~~";
    public string Color(string text, string color) => text; // Discord doesn't support colors in text
    public string Size(string text, string size) => text; // Discord doesn't support text size
    public string Font(string text, string fontName) => text; // Discord doesn't support fonts

    // Links
    public string Url(string url, string? text = null)
    {
        if (string.IsNullOrWhiteSpace(text) || text == url)
            return $"<{url}>"; // Suppress embed
        return $"[{text}]({url})";
    }

    public string Email(string email, string? displayText = null)
    {
        var text = string.IsNullOrWhiteSpace(displayText) ? email : displayText;
        return $"[{text}](mailto:{email})";
    }

    public string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"@{clean}";
    }

    // Discord user ID mention
    public string MentionById(string userId) => $"<@{userId}>";
    public string MentionRole(string roleId) => $"<@&{roleId}>";
    public string MentionChannel(string channelId) => $"<#{channelId}>";

    // Media - Discord auto-embeds
    public string Image(string url, string? caption = null)
    {
        var result = url;
        if (!string.IsNullOrWhiteSpace(caption))
            result += $"\n*{caption}*";
        return result;
    }

    public string Video(string url) => url;
    public string Audio(string url) => url;
    public string YouTube(string videoIdOrUrl) => ExtractYouTubeUrl(videoIdOrUrl);

    // Structure
    public string Quote(string text, string? author = null)
    {
        var lines = text.Split('\n').Select(l => $"> {l}");
        var quoted = string.Join("\n", lines);
        if (!string.IsNullOrWhiteSpace(author))
            quoted = $"**{author}:**\n{quoted}";
        return quoted;
    }

    public string Code(string code, string? language = null)
    {
        if (!code.Contains('\n'))
            return $"`{code}`"; // Inline code
        var lang = language ?? "";
        return $"```{lang}\n{code}\n```";
    }

    public string Spoiler(string text, string? title = null)
    {
        // Discord spoiler syntax
        var result = $"||{text}||";
        if (!string.IsNullOrWhiteSpace(title))
            result = $"**{title}:** {result}";
        return result;
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
            return $"- {i}";
        });
        return string.Join("\n", listItems);
    }

    public string Table(string[,] cells, bool hasHeader = true)
    {
        // Discord doesn't support tables natively, use code block
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var colWidths = new int[cols];

        // Calculate column widths
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
            for (int c = 0; c < cols; c++)
            {
                var cell = cells[r, c] ?? "";
                sb.Append(cell.PadRight(colWidths[c] + 2));
            }
            sb.AppendLine();

            // Add separator after header
            if (hasHeader && r == 0)
            {
                for (int c = 0; c < cols; c++)
                {
                    sb.Append(new string('-', colWidths[c]));
                    sb.Append("  ");
                }
                sb.AppendLine();
            }
        }

        sb.AppendLine("```");
        return sb.ToString().TrimEnd();
    }

    public string Align(string text, TextAlignment alignment) => text; // Not supported
    public string Header(string text, HeaderLevel level)
    {
        // Discord uses markdown headers in embeds, bold for regular messages
        var prefix = level switch
        {
            HeaderLevel.H1 => "# ",
            HeaderLevel.H2 => "## ",
            HeaderLevel.H3 => "### ",
            _ => "**"
        };
        var suffix = (int)level > 3 ? "**" : "";
        return $"{prefix}{text}{suffix}";
    }

    public string HorizontalRule() => "\n─────────────────\n";

    // Special
    public string ProgressBar(int percent, string? label = null)
    {
        var pct = Math.Clamp(percent, 0, 100);
        var filled = pct / 10;
        var bar = $"[{'█'.ToString().PadRight(filled, '█')}{'░'.ToString().PadRight(10 - filled, '░')}]";
        return $"{bar} {label ?? $"{pct}%"}";
    }

    public string Marquee(string text) => text; // Not supported
    public string Hide(string text, string? buttonText = null) => Spoiler(text, buttonText);

    // Discord-specific: Timestamp
    public string Timestamp(DateTime dateTime, string format = "f")
    {
        var unix = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        return $"<t:{unix}:{format}>";
    }

    // Discord-specific: Custom emoji
    public string CustomEmoji(string name, string id, bool animated = false)
    {
        var prefix = animated ? "a" : "";
        return $"<{prefix}:{name}:{id}>";
    }

    public string FormatText(string text, bool bold = false, bool italic = false,
        bool underline = false, bool strikethrough = false,
        string? color = null, string? size = null)
    {
        var result = text;
        if (strikethrough) result = Strikethrough(result);
        if (underline) result = Underline(result);
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
