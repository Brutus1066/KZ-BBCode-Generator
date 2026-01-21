using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Discourse markdown-based generator
/// Uses standard Markdown with Discourse extensions
/// </summary>
public class DiscourseGen : IBBCodeGen
{
    public PlatformType Platform => PlatformType.Discourse;

    // Text formatting - Markdown
    public string Bold(string text) => $"**{text}**";
    public string Italic(string text) => $"*{text}*";
    public string Underline(string text) => $"<u>{text}</u>"; // HTML fallback
    public string Strikethrough(string text) => $"~~{text}~~";
    public string Color(string text, string color) => $"<span style=\"color:{color}\">{text}</span>";
    public string Size(string text, string size) => $"<span style=\"font-size:{size}px\">{text}</span>";
    public string Font(string text, string fontName) => $"<span style=\"font-family:{fontName}\">{text}</span>";

    // Links
    public string Url(string url, string? text = null)
    {
        var linkText = string.IsNullOrWhiteSpace(text) ? url : text;
        return $"[{linkText}]({url})";
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

    // Media
    public string Image(string url, string? caption = null)
    {
        var result = $"![image]({url})";
        if (!string.IsNullOrWhiteSpace(caption))
            result += $"\n*{caption}*";
        return result;
    }

    public string Video(string url) => url; // Auto-embeds
    public string Audio(string url) => $"<audio controls src=\"{url}\"></audio>";

    public string YouTube(string videoIdOrUrl)
    {
        var videoId = ExtractYouTubeId(videoIdOrUrl);
        return $"https://www.youtube.com/watch?v={videoId}";
    }

    // Structure
    public string Quote(string text, string? author = null)
    {
        var lines = text.Split('\n').Select(l => $"> {l}");
        var quoted = string.Join("\n", lines);
        if (!string.IsNullOrWhiteSpace(author))
            return $"[quote=\"{author}\"]\n{text}\n[/quote]"; // Discourse quote syntax
        return quoted;
    }

    public string Code(string code, string? language = null)
    {
        var lang = language ?? "";
        return $"```{lang}\n{code}\n```";
    }

    public string Spoiler(string text, string? title = null)
    {
        var summary = string.IsNullOrWhiteSpace(title) ? "Click to reveal" : title;
        return $"<details>\n<summary>{summary}</summary>\n\n{text}\n\n</details>";
    }

    public string List(IEnumerable<string> items, ListType type = ListType.Bullet)
    {
        var prefix = type == ListType.Bullet ? "-" : "1.";
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
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var sb = new System.Text.StringBuilder();

        // Header row
        sb.Append("|");
        for (int c = 0; c < cols; c++)
            sb.Append($" {cells[0, c]} |");
        sb.AppendLine();

        // Separator
        sb.Append("|");
        for (int c = 0; c < cols; c++)
            sb.Append(" --- |");
        sb.AppendLine();

        // Data rows
        for (int r = 1; r < rows; r++)
        {
            sb.Append("|");
            for (int c = 0; c < cols; c++)
                sb.Append($" {cells[r, c]} |");
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    public string Align(string text, TextAlignment alignment)
    {
        var align = alignment.ToString().ToLower();
        return $"<div style=\"text-align:{align}\">{text}</div>";
    }

    public string Header(string text, HeaderLevel level)
    {
        var hashes = new string('#', (int)level);
        return $"{hashes} {text}";
    }

    public string HorizontalRule() => "\n---\n";

    // Special
    public string ProgressBar(int percent, string? label = null)
    {
        var pct = Math.Clamp(percent, 0, 100);
        return $"[|{'='.ToString().PadRight(pct / 5, '=')}|] {label ?? $"{pct}%"}";
    }

    public string Marquee(string text) => text; // Not supported
    public string Hide(string text, string? buttonText = null) => Spoiler(text, buttonText);

    public string FormatText(string text, bool bold = false, bool italic = false,
        bool underline = false, bool strikethrough = false,
        string? color = null, string? size = null)
    {
        var result = text;
        if (strikethrough) result = Strikethrough(result);
        if (underline) result = Underline(result);
        if (italic) result = Italic(result);
        if (bold) result = Bold(result);
        if (!string.IsNullOrWhiteSpace(size)) result = Size(result, size);
        if (!string.IsNullOrWhiteSpace(color)) result = Color(result, color);
        return result;
    }

    private static string ExtractYouTubeId(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return url;
        if (url.Contains("youtu.be/"))
        {
            var idx = url.IndexOf("youtu.be/") + 9;
            var end = url.IndexOf('?', idx);
            return end > 0 ? url[idx..end] : url[idx..];
        }
        if (url.Contains("v="))
        {
            var idx = url.IndexOf("v=") + 2;
            var end = url.IndexOf('&', idx);
            return end > 0 ? url[idx..end] : url[idx..];
        }
        return url;
    }
}
