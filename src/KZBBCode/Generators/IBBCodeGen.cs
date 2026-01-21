using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Interface for platform-specific BBCode and Markdown generators.
/// </summary>
/// <remarks>
/// <para>This interface defines the contract for all formatting generators in the application.
/// Each platform (phpBB, Discord, Slack, etc.) implements this interface to provide
/// platform-specific formatting syntax.</para>
/// 
/// <para><b>Implementation Pattern:</b> Most implementations inherit from <see cref="BBCodeBase"/>
/// and override only the methods that differ from standard BBCode syntax.</para>
/// 
/// <para><b>Categories of Methods:</b></para>
/// <list type="bullet">
///   <item><description><b>Text Formatting</b> - Bold, italic, underline, strikethrough, color, size, font</description></item>
///   <item><description><b>Links &amp; Media</b> - URLs, emails, mentions, images, videos, audio, YouTube</description></item>
///   <item><description><b>Structure</b> - Quotes, code blocks, spoilers, lists, tables, alignment, headers</description></item>
///   <item><description><b>Special</b> - Progress bars, marquee, hide/reveal sections</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// // Get a generator and format text
/// IBBCodeGen gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
/// string output = gen.Bold("Hello") + " " + gen.Italic("World");
/// // Discord output: **Hello** *World*
/// </code>
/// </example>
public interface IBBCodeGen
{
    /// <summary>Gets the platform type this generator targets.</summary>
    PlatformType Platform { get; }

    #region Text Formatting

    /// <summary>Applies bold formatting to text.</summary>
    /// <param name="text">The text to make bold.</param>
    /// <returns>Formatted string with bold syntax (e.g., [b]text[/b] or **text**).</returns>
    string Bold(string text);

    /// <summary>Applies italic formatting to text.</summary>
    /// <param name="text">The text to italicize.</param>
    /// <returns>Formatted string with italic syntax.</returns>
    string Italic(string text);

    /// <summary>Applies underline formatting to text.</summary>
    /// <param name="text">The text to underline.</param>
    /// <returns>Formatted string with underline syntax.</returns>
    string Underline(string text);

    /// <summary>Applies strikethrough formatting to text.</summary>
    /// <param name="text">The text to strike through.</param>
    /// <returns>Formatted string with strikethrough syntax.</returns>
    string Strikethrough(string text);

    /// <summary>Applies color formatting to text.</summary>
    /// <param name="text">The text to colorize.</param>
    /// <param name="color">Color name (e.g., "red") or hex code (e.g., "#FF0000").</param>
    /// <returns>Formatted string with color syntax.</returns>
    string Color(string text, string color);

    /// <summary>Applies font size formatting to text.</summary>
    /// <param name="text">The text to resize.</param>
    /// <param name="size">Size value (platform-dependent, typically 1-7 or pixel values).</param>
    /// <returns>Formatted string with size syntax.</returns>
    string Size(string text, string size);

    /// <summary>Applies font family formatting to text.</summary>
    /// <param name="text">The text to format.</param>
    /// <param name="fontName">Font family name (e.g., "Arial", "Courier").</param>
    /// <returns>Formatted string with font syntax.</returns>
    string Font(string text, string fontName);

    #endregion

    #region Links and Media

    /// <summary>Creates a hyperlink.</summary>
    /// <param name="url">The target URL.</param>
    /// <param name="text">Optional display text. If null, the URL is shown.</param>
    /// <returns>Formatted hyperlink.</returns>
    string Url(string url, string? text = null);

    /// <summary>Creates a mailto email link.</summary>
    /// <param name="email">The email address.</param>
    /// <param name="displayText">Optional display text.</param>
    /// <returns>Formatted email link.</returns>
    string Email(string email, string? displayText = null);

    /// <summary>Creates a user mention/tag.</summary>
    /// <param name="username">Username to mention (with or without @ prefix).</param>
    /// <returns>Formatted mention.</returns>
    string Mention(string username);

    /// <summary>Embeds an image.</summary>
    /// <param name="url">URL to the image.</param>
    /// <param name="caption">Optional caption text.</param>
    /// <returns>Formatted image embed.</returns>
    string Image(string url, string? caption = null);

    /// <summary>Embeds a video.</summary>
    /// <param name="url">URL to the video.</param>
    /// <returns>Formatted video embed.</returns>
    string Video(string url);

    /// <summary>Embeds audio content.</summary>
    /// <param name="url">URL to the audio file.</param>
    /// <returns>Formatted audio embed.</returns>
    string Audio(string url);

    /// <summary>Embeds a YouTube video.</summary>
    /// <param name="videoIdOrUrl">YouTube video ID or full URL.</param>
    /// <returns>Formatted YouTube embed.</returns>
    string YouTube(string videoIdOrUrl);

    #endregion

    #region Structure

    /// <summary>Creates a quote block.</summary>
    /// <param name="text">The quoted text.</param>
    /// <param name="author">Optional author attribution.</param>
    /// <returns>Formatted quote block.</returns>
    string Quote(string text, string? author = null);

    /// <summary>Creates a code block.</summary>
    /// <param name="code">The code content.</param>
    /// <param name="language">Optional language for syntax highlighting.</param>
    /// <returns>Formatted code block.</returns>
    string Code(string code, string? language = null);

    /// <summary>Creates a spoiler/hidden section.</summary>
    /// <param name="text">The hidden content.</param>
    /// <param name="title">Optional spoiler button title.</param>
    /// <returns>Formatted spoiler block.</returns>
    string Spoiler(string text, string? title = null);

    /// <summary>Creates a formatted list.</summary>
    /// <param name="items">List items.</param>
    /// <param name="type">List type (bullet, numbered, or lettered).</param>
    /// <returns>Formatted list.</returns>
    string List(IEnumerable<string> items, ListType type = ListType.Bullet);

    /// <summary>Creates a formatted table.</summary>
    /// <param name="cells">2D array of cell contents [rows, columns].</param>
    /// <param name="hasHeader">Whether the first row is a header row.</param>
    /// <returns>Formatted table.</returns>
    string Table(string[,] cells, bool hasHeader = true);

    /// <summary>Applies text alignment.</summary>
    /// <param name="text">The text to align.</param>
    /// <param name="alignment">Alignment direction.</param>
    /// <returns>Formatted aligned text.</returns>
    string Align(string text, TextAlignment alignment);

    /// <summary>Creates a header/title.</summary>
    /// <param name="text">The header text.</param>
    /// <param name="level">Header level (H1-H6).</param>
    /// <returns>Formatted header.</returns>
    string Header(string text, HeaderLevel level);

    /// <summary>Creates a horizontal rule/separator.</summary>
    /// <returns>Formatted horizontal rule.</returns>
    string HorizontalRule();

    #endregion

    #region Special

    /// <summary>Creates a progress bar (platform support varies).</summary>
    /// <param name="percent">Percentage complete (0-100).</param>
    /// <param name="label">Optional label text.</param>
    /// <returns>Formatted progress bar.</returns>
    string ProgressBar(int percent, string? label = null);

    /// <summary>Creates scrolling/marquee text (legacy feature).</summary>
    /// <param name="text">The scrolling text.</param>
    /// <returns>Formatted marquee.</returns>
    string Marquee(string text);

    /// <summary>Creates a hide/reveal section.</summary>
    /// <param name="text">The hidden content.</param>
    /// <param name="buttonText">Optional button label.</param>
    /// <returns>Formatted hide section.</returns>
    string Hide(string text, string? buttonText = null);

    #endregion

    #region Combined Formatting

    /// <summary>Applies multiple formatting options at once.</summary>
    /// <param name="text">The text to format.</param>
    /// <param name="bold">Apply bold formatting.</param>
    /// <param name="italic">Apply italic formatting.</param>
    /// <param name="underline">Apply underline formatting.</param>
    /// <param name="strikethrough">Apply strikethrough formatting.</param>
    /// <param name="color">Optional color value.</param>
    /// <param name="size">Optional size value.</param>
    /// <returns>Text with all specified formatting applied.</returns>
    string FormatText(string text, bool bold = false, bool italic = false,
        bool underline = false, bool strikethrough = false,
        string? color = null, string? size = null);

    #endregion
}
