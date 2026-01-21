using System.Text.Json.Serialization;

namespace KZBBCode.Models;

/// <summary>
/// Application settings persisted to JSON in %AppData%/KZBBCode/settings.json.
/// </summary>
/// <remarks>
/// <para>This class contains all user preferences and state that should persist between sessions.
/// Settings are automatically loaded on startup and saved on exit or when explicitly requested.</para>
/// 
/// <para><b>Categories:</b></para>
/// <list type="bullet">
///   <item><description><b>Appearance:</b> Theme, colors, font sizes</description></item>
///   <item><description><b>Behavior:</b> Auto-copy, platform selection</description></item>
///   <item><description><b>Window State:</b> Position, size, maximized state</description></item>
///   <item><description><b>History:</b> Recent generated codes (session-based)</description></item>
/// </list>
/// </remarks>
public class AppSettings
{
    /// <summary>Name of the selected theme (e.g., "Dark Mode", "Dracula").</summary>
    public string ThemeName { get; set; } = "Dark Mode";

    /// <summary>Name of the selected platform (e.g., "phpBB", "Discord").</summary>
    public string PlatformName { get; set; } = "phpBB";

    /// <summary>Whether to automatically copy generated code to clipboard.</summary>
    public bool AutoCopyToClipboard { get; set; } = true;

    /// <summary>Whether to show live preview (reserved for future use).</summary>
    public bool ShowPreview { get; set; } = true;

    /// <summary>Whether to remember and restore window position on startup.</summary>
    public bool RememberWindowPosition { get; set; } = true;

    /// <summary>Last saved window position and size.</summary>
    public WindowPosition? LastWindowPosition { get; set; }

    /// <summary>History of generated codes for the current session.</summary>
    public List<string> SessionHistory { get; set; } = new();

    /// <summary>Maximum number of items to keep in history.</summary>
    public int MaxHistoryItems { get; set; } = 50;

    /// <summary>Whether the user has been asked about creating a desktop shortcut.</summary>
    public bool AskedAboutShortcut { get; set; } = false;

    /// <summary>Whether to create a desktop shortcut on first run.</summary>
    public bool CreateDesktopShortcut { get; set; } = false;

    /// <summary>Default font size for size formatting (1-7 for BBCode).</summary>
    public string DefaultFontSize { get; set; } = "3";

    /// <summary>Default color for color formatting (empty = no default).</summary>
    public string DefaultColor { get; set; } = "";

    /// <summary>Timestamp of last application use.</summary>
    public DateTime LastUsed { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets the platform type from the platform name.
    /// </summary>
    /// <remarks>Parses the PlatformName string to a PlatformType enum, falling back to phpBB.</remarks>
    [JsonIgnore]
    public PlatformType Platform => Enum.TryParse<PlatformType>(PlatformName.Replace("/", "").Replace(" ", ""), true, out var p)
        ? p
        : PlatformType.PhpBB;
}

/// <summary>
/// Window position and size for persistence across sessions.
/// </summary>
public class WindowPosition
{
    /// <summary>Window X coordinate (left edge).</summary>
    public int X { get; set; }

    /// <summary>Window Y coordinate (top edge).</summary>
    public int Y { get; set; }

    /// <summary>Window width in pixels.</summary>
    public int Width { get; set; }

    /// <summary>Window height in pixels.</summary>
    public int Height { get; set; }

    /// <summary>Whether the window was maximized.</summary>
    public bool Maximized { get; set; }
}
