using System.Text.Json;
using KZBBCode.Models;

namespace KZBBCode.Services;

/// <summary>
/// Service for persisting and loading application settings to/from JSON.
/// </summary>
/// <remarks>
/// <para>Settings are stored in <c>%AppData%/KZBBCode/settings.json</c>.</para>
/// <para>The service provides caching to avoid repeated disk reads, and silently
/// handles errors to ensure the application remains functional even if settings
/// cannot be saved or loaded.</para>
/// </remarks>
public static class SettingsService
{
    #region Private Fields

    private static readonly string SettingsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "KZBBCode"
    );

    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "settings.json");

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static AppSettings? _cachedSettings;

    #endregion

    #region Load and Save

    /// <summary>
    /// Loads settings from disk or returns cached/default settings.
    /// </summary>
    /// <returns>The loaded settings, or default settings if loading fails.</returns>
    public static AppSettings Load()
    {
        if (_cachedSettings != null)
            return _cachedSettings;

        try
        {
            if (File.Exists(SettingsFile))
            {
                var json = File.ReadAllText(SettingsFile);
                _cachedSettings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            }
            else
            {
                _cachedSettings = new AppSettings();
            }
        }
        catch
        {
            _cachedSettings = new AppSettings();
        }

        return _cachedSettings;
    }

    /// <summary>
    /// Saves settings to disk, creating the settings folder if needed.
    /// </summary>
    /// <param name="settings">The settings to save.</param>
    public static void Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(SettingsFolder);
            settings.LastUsed = DateTime.Now;
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            File.WriteAllText(SettingsFile, json);
            _cachedSettings = settings;
        }
        catch
        {
            // Silently fail - settings are not critical
        }
    }

    #endregion

    #region History Management

    /// <summary>
    /// Adds a generated code entry to the session history with timestamp.
    /// </summary>
    /// <param name="settings">The settings object to update.</param>
    /// <param name="generatedCode">The code to add to history.</param>
    public static void AddToHistory(AppSettings settings, string generatedCode)
    {
        if (string.IsNullOrWhiteSpace(generatedCode))
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var entry = $"[{timestamp}]\n{generatedCode}";

        settings.SessionHistory.Insert(0, entry);

        // Trim history to max items
        while (settings.SessionHistory.Count > settings.MaxHistoryItems)
        {
            settings.SessionHistory.RemoveAt(settings.SessionHistory.Count - 1);
        }
    }

    /// <summary>
    /// Clears all session history entries.
    /// </summary>
    /// <param name="settings">The settings object to clear.</param>
    public static void ClearHistory(AppSettings settings)
    {
        settings.SessionHistory.Clear();
    }

    /// <summary>
    /// Exports the session history to a text file.
    /// </summary>
    /// <param name="settings">The settings containing the history.</param>
    /// <param name="filePath">Destination file path.</param>
    /// <returns><c>true</c> if export succeeded; otherwise, <c>false</c>.</returns>
    public static bool ExportHistory(AppSettings settings, string filePath)
    {
        try
        {
            var content = string.Join("\n\n" + new string('=', 50) + "\n\n", settings.SessionHistory);
            File.WriteAllText(filePath, content);
            return true;
        }
        catch
        {
            return false;
        }
    }

    #endregion

    #region Window State Persistence

    /// <summary>
    /// Saves the current window position and size to settings.
    /// </summary>
    /// <param name="settings">The settings object to update.</param>
    /// <param name="form">The form whose position to save.</param>
    public static void SaveWindowPosition(AppSettings settings, Form form)
    {
        if (!settings.RememberWindowPosition)
            return;

        settings.LastWindowPosition = new WindowPosition
        {
            X = form.Location.X,
            Y = form.Location.Y,
            Width = form.Width,
            Height = form.Height,
            Maximized = form.WindowState == FormWindowState.Maximized
        };
    }

    /// <summary>
    /// Restores the window position and size from settings.
    /// </summary>
    /// <remarks>Validates that the position is still on-screen before applying.</remarks>
    /// <param name="settings">The settings containing saved position.</param>
    /// <param name="form">The form to restore.</param>
    public static void RestoreWindowPosition(AppSettings settings, Form form)
    {
        if (!settings.RememberWindowPosition || settings.LastWindowPosition == null)
            return;

        var pos = settings.LastWindowPosition;

        // Validate position is on screen
        var screenBounds = Screen.FromPoint(new Point(pos.X, pos.Y)).WorkingArea;
        if (pos.X >= screenBounds.Left && pos.X < screenBounds.Right &&
            pos.Y >= screenBounds.Top && pos.Y < screenBounds.Bottom)
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(pos.X, pos.Y);
        }

        if (pos.Width > 100 && pos.Height > 100)
        {
            form.Size = new Size(pos.Width, pos.Height);
        }

        if (pos.Maximized)
        {
            form.WindowState = FormWindowState.Maximized;
        }
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Gets the path to the settings folder (%AppData%/KZBBCode).
    /// </summary>
    /// <returns>Absolute path to settings folder.</returns>
    public static string GetSettingsFolder() => SettingsFolder;

    #endregion
}
