using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Abstract base class providing default BBCode implementations for all formatting methods.
/// </summary>
/// <remarks>
/// <para>This class implements the <see cref="IBBCodeGen"/> interface with standard BBCode syntax.
/// Platform-specific generators inherit from this class and override only the methods that
/// differ from the default BBCode format.</para>
/// 
/// <para><b>Design Pattern:</b> Template Method - provides a skeleton implementation where
/// subclasses can override specific steps without changing the overall structure.</para>
/// 
/// <para><b>Default Tag Format:</b> <c>[tag]content[/tag]</c> or <c>[tag=value]content[/tag]</c></para>
/// </remarks>
/// <example>
/// <code>
/// // Create a custom platform generator
/// public class MyForumGen : BBCodeBase
/// {
///     public override PlatformType Platform => PlatformType.MyBB;
///     
///     // Override only methods that differ from standard BBCode
///     public override string Spoiler(string text, string? title = null)
///         => $"[spoiler]{text}[/spoiler]"; // Simplified version
/// }
/// </code>
/// </example>
public abstract class BBCodeBase : IBBCodeGen
{
    /// <summary>Gets the platform type this generator targets.</summary>
    public abstract PlatformType Platform { get; }

    #region Text Formatting

    /// <inheritdoc />
    public virtual string Bold(string text) => $"[b]{text}[/b]";

    /// <inheritdoc />
    public virtual string Italic(string text) => $"[i]{text}[/i]";

    /// <inheritdoc />
    public virtual string Underline(string text) => $"[u]{text}[/u]";

    /// <inheritdoc />
    public virtual string Strikethrough(string text) => $"[s]{text}[/s]";

    /// <inheritdoc />
    public virtual string Color(string text, string color) => $"[color={color}]{text}[/color]";

    /// <inheritdoc />
    public virtual string Size(string text, string size) => $"[size={size}]{text}[/size]";

    /// <inheritdoc />
    public virtual string Font(string text, string fontName) => $"[font={fontName}]{text}[/font]";

    #endregion

    #region Links and Media

    /// <inheritdoc />
    public virtual string Url(string url, string? text = null)
    {
        var linkText = string.IsNullOrWhiteSpace(text) ? url : text;
        return $"[url={url}]{linkText}[/url]";
    }

    /// <inheritdoc />
    public virtual string Email(string email, string? displayText = null)
    {
        var text = string.IsNullOrWhiteSpace(displayText) ? email : displayText;
        return $"[email={email}]{text}[/email]";
    }

    /// <inheritdoc />
    public virtual string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"@{clean}";
    }

    /// <inheritdoc />
    public virtual string Image(string url, string? caption = null)
    {
        var result = $"[img]{url}[/img]";
        if (!string.IsNullOrWhiteSpace(caption))
            result += $"\n[i]{caption}[/i]";
        return result;
    }

    /// <inheritdoc />
    public virtual string Video(string url) => $"[video]{url}[/video]";

    /// <inheritdoc />
    public virtual string Audio(string url) => $"[audio]{url}[/audio]";

    /// <inheritdoc />
    public virtual string YouTube(string videoIdOrUrl)
    {
        var videoId = ExtractYouTubeId(videoIdOrUrl);
        return $"[youtube]{videoId}[/youtube]";
    }

    #endregion

    #region Structure

    /// <inheritdoc />
    public virtual string Quote(string text, string? author = null)
    {
        if (!string.IsNullOrWhiteSpace(author))
            return $"[quote={author}]{text}[/quote]";
        return $"[quote]{text}[/quote]";
    }

    /// <inheritdoc />
    public virtual string Code(string code, string? language = null)
    {
        if (!string.IsNullOrWhiteSpace(language))
            return $"[code={language}]\n{code}\n[/code]";
        return $"[code]\n{code}\n[/code]";
    }

    /// <inheritdoc />
    public virtual string Spoiler(string text, string? title = null)
    {
        if (!string.IsNullOrWhiteSpace(title))
            return $"[spoiler={title}]{text}[/spoiler]";
        return $"[spoiler]{text}[/spoiler]";
    }

    /// <inheritdoc />
    public virtual string List(IEnumerable<string> items, ListType type = ListType.Bullet)
    {
        var listTag = type switch
        {
            ListType.Numbered => "[list=1]",
            ListType.Lettered => "[list=a]",
            _ => "[list]"
        };

        var itemsText = string.Join("\n", items.Select(i => $"[*]{i}"));
        return $"{listTag}\n{itemsText}\n[/list]";
    }

    /// <inheritdoc />
    public virtual string Table(string[,] cells, bool hasHeader = true)
    {
        var rows = cells.GetLength(0);
        var cols = cells.GetLength(1);
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("[table]");
        for (int r = 0; r < rows; r++)
        {
            sb.AppendLine("[tr]");
            for (int c = 0; c < cols; c++)
            {
                var tag = (hasHeader && r == 0) ? "th" : "td";
                sb.AppendLine($"[{tag}]{cells[r, c]}[/{tag}]");
            }
            sb.AppendLine("[/tr]");
        }
        sb.AppendLine("[/table]");
        return sb.ToString().TrimEnd();
    }

    /// <inheritdoc />
    public virtual string Align(string text, TextAlignment alignment)
    {
        var tag = alignment switch
        {
            TextAlignment.Left => "left",
            TextAlignment.Center => "center",
            TextAlignment.Right => "right",
            TextAlignment.Justify => "justify",
            _ => "left"
        };
        return $"[{tag}]{text}[/{tag}]";
    }

    /// <inheritdoc />
    public virtual string Header(string text, HeaderLevel level)
    {
        var size = level switch
        {
            HeaderLevel.H1 => "7",
            HeaderLevel.H2 => "6",
            HeaderLevel.H3 => "5",
            HeaderLevel.H4 => "4",
            HeaderLevel.H5 => "3",
            HeaderLevel.H6 => "2",
            _ => "5"
        };
        return Bold(Size(text, size));
    }

    /// <inheritdoc />
    public virtual string HorizontalRule() => "[hr]";

    #endregion

    #region Special

    /// <inheritdoc />
    public virtual string ProgressBar(int percent, string? label = null)
    {
        var pct = Math.Clamp(percent, 0, 100);
        var bar = $"[progress={pct}]{label ?? $"{pct}%"}[/progress]";
        return bar;
    }

    /// <inheritdoc />
    public virtual string Marquee(string text) => $"[marquee]{text}[/marquee]";

    /// <inheritdoc />
    public virtual string Hide(string text, string? buttonText = null)
    {
        if (!string.IsNullOrWhiteSpace(buttonText))
            return $"[hide={buttonText}]{text}[/hide]";
        return $"[hide]{text}[/hide]";
    }

    #endregion

    #region Combined Formatting

    /// <inheritdoc />
    public virtual string FormatText(string text, bool bold = false, bool italic = false,
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

    #endregion

    #region Helper Methods

    /// <summary>
    /// Extracts the YouTube video ID from a URL or returns the input if already an ID.
    /// </summary>
    /// <param name="url">YouTube URL or video ID.</param>
    /// <returns>The 11-character video ID.</returns>
    protected static string ExtractYouTubeId(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return url;

        // Short URL format
        if (url.Contains("youtu.be/"))
        {
            var idx = url.IndexOf("youtu.be/") + 9;
            var end = url.IndexOf('?', idx);
            return end > 0 ? url[idx..end] : url[idx..];
        }

        // Standard URL format
        if (url.Contains("v="))
        {
            var idx = url.IndexOf("v=") + 2;
            var end = url.IndexOf('&', idx);
            return end > 0 ? url[idx..end] : url[idx..];
        }

        // Assume it's already a video ID
        return url;
    }

    #endregion
}
