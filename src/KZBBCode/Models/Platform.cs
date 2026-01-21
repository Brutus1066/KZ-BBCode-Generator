namespace KZBBCode.Models;

/// <summary>
/// Enumeration of all supported forum and chat platforms.
/// </summary>
/// <remarks>
/// <para>Each platform type maps to a specific generator implementation
/// that produces correctly formatted output for that platform.</para>
/// </remarks>
public enum PlatformType
{
    /// <summary>phpBB - Classic open-source forum software.</summary>
    PhpBB,

    /// <summary>vBulletin - Popular commercial forum platform.</summary>
    VBulletin,

    /// <summary>MyBB - Free and open-source forum software.</summary>
    MyBB,

    /// <summary>Classic BBCode - Traditional forum standard format.</summary>
    ClassicBBCode,

    /// <summary>SMF - Simple Machines Forum.</summary>
    SMF,

    /// <summary>IPB - Invision Power Board (Invision Community).</summary>
    IPB,

    /// <summary>XenForo - Modern commercial forum software.</summary>
    XenForo,

    /// <summary>Discourse - Modern open-source discussion platform.</summary>
    Discourse,

    /// <summary>Discord - Gaming and community chat platform.</summary>
    Discord,

    /// <summary>Slack - Workplace messaging platform.</summary>
    Slack
}

/// <summary>
/// Platform metadata containing display information and format configuration.
/// </summary>
/// <remarks>
/// <para>This record provides immutable platform information used throughout the application
/// for display names, descriptions, categorization, and format detection.</para>
/// </remarks>
/// <param name="Type">The platform type enumeration value.</param>
/// <param name="Name">Display name shown in the UI dropdown.</param>
/// <param name="Description">Short description for tooltips and help.</param>
/// <param name="Category">Platform category (Legacy, Modern, Chat).</param>
/// <param name="Format">The markup format used by this platform.</param>
public record PlatformInfo(
    PlatformType Type,
    string Name,
    string Description,
    PlatformCategory Category,
    FormatType Format
)
{
    /// <summary>
    /// Complete list of all supported platforms with their metadata.
    /// </summary>
    public static readonly IReadOnlyList<PlatformInfo> All = new[]
    {
        new PlatformInfo(PlatformType.PhpBB, "phpBB", "Classic forum software", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.VBulletin, "vBulletin", "Popular commercial forum", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.MyBB, "MyBB", "Free and open source forum", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.ClassicBBCode, "Classic BBCode", "Traditional forum standard", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.SMF, "SMF", "Simple Machines Forum", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.IPB, "IPB/Invision", "Invision Power Board", PlatformCategory.Legacy, FormatType.BBCode),
        new PlatformInfo(PlatformType.XenForo, "XenForo", "Modern forum software", PlatformCategory.Modern, FormatType.BBCode),
        new PlatformInfo(PlatformType.Discourse, "Discourse", "Modern discussion platform", PlatformCategory.Modern, FormatType.Markdown),
        new PlatformInfo(PlatformType.Discord, "Discord", "Gaming & community chat", PlatformCategory.Chat, FormatType.Markdown),
        new PlatformInfo(PlatformType.Slack, "Slack", "Workplace messaging", PlatformCategory.Chat, FormatType.SlackMarkdown)
    };

    /// <summary>
    /// Gets platform info by type enumeration.
    /// </summary>
    /// <param name="type">The platform type to find.</param>
    /// <returns>Platform info, or phpBB as fallback.</returns>
    public static PlatformInfo GetByType(PlatformType type) =>
        All.FirstOrDefault(p => p.Type == type) ?? All[0];

    /// <summary>
    /// Gets platform info by display name.
    /// </summary>
    /// <param name="name">The platform name to find (case-insensitive).</param>
    /// <returns>Platform info, or phpBB as fallback.</returns>
    public static PlatformInfo GetByName(string name) =>
        All.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? All[0];
}

/// <summary>
/// Platform category for grouping in the UI.
/// </summary>
public enum PlatformCategory
{
    /// <summary>Traditional BBCode forums (phpBB, vBulletin, etc.).</summary>
    Legacy,

    /// <summary>Modern discussion platforms (Discourse, XenForo).</summary>
    Modern,

    /// <summary>Real-time chat applications (Discord, Slack).</summary>
    Chat
}

/// <summary>
/// Markup format type used by a platform.
/// </summary>
public enum FormatType
{
    /// <summary>Standard BBCode format with [tag]content[/tag] syntax.</summary>
    BBCode,

    /// <summary>CommonMark/GitHub Flavored Markdown.</summary>
    Markdown,

    /// <summary>Slack's proprietary mrkdwn format.</summary>
    SlackMarkdown
}

/// <summary>
/// List formatting types for bullet, numbered, and lettered lists.
/// </summary>
public enum ListType
{
    /// <summary>Unordered bullet list (• item).</summary>
    Bullet,

    /// <summary>Numbered list (1. 2. 3.).</summary>
    Numbered,

    /// <summary>Lettered list (a. b. c.).</summary>
    Lettered
}

/// <summary>
/// Text alignment options for paragraphs and blocks.
/// </summary>
public enum TextAlignment
{
    /// <summary>Left-aligned text (default).</summary>
    Left,

    /// <summary>Center-aligned text.</summary>
    Center,

    /// <summary>Right-aligned text.</summary>
    Right,

    /// <summary>Justified text (even margins).</summary>
    Justify
}

/// <summary>
/// Header levels (H1-H6) for title formatting.
/// </summary>
public enum HeaderLevel
{
    /// <summary>Heading level 1 (largest).</summary>
    H1 = 1,

    /// <summary>Heading level 2.</summary>
    H2 = 2,

    /// <summary>Heading level 3.</summary>
    H3 = 3,

    /// <summary>Heading level 4.</summary>
    H4 = 4,

    /// <summary>Heading level 5.</summary>
    H5 = 5,

    /// <summary>Heading level 6 (smallest).</summary>
    H6 = 6
}
