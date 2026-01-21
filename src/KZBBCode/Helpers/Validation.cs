using System.Text.RegularExpressions;

namespace KZBBCode.Helpers;

/// <summary>
/// Static helper class providing input validation methods for URLs, emails, colors, and other user inputs.
/// </summary>
/// <remarks>
/// <para>This class uses source-generated regular expressions for optimal performance.
/// All validation methods are null-safe and return false for null/empty inputs.</para>
/// 
/// <para><b>Validation Categories:</b></para>
/// <list type="bullet">
///   <item><description><b>URLs:</b> General URLs, image URLs, YouTube URLs</description></item>
///   <item><description><b>Formatting:</b> Colors (hex and named), sizes</description></item>
///   <item><description><b>Communication:</b> Email addresses</description></item>
///   <item><description><b>Sanitization:</b> Input cleaning and normalization</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// if (Validation.IsValidUrl(userInput))
/// {
///     generator.Url(userInput, "Click here");
/// }
/// 
/// var color = Validation.NormalizeColor("#ff0000"); // Returns "#FF0000"
/// </code>
/// </example>
public static partial class Validation
{
    #region URL Validation

    /// <summary>
    /// Validates that a string is a properly formatted HTTP or HTTPS URL.
    /// </summary>
    /// <param name="url">The URL string to validate.</param>
    /// <returns><c>true</c> if the URL is valid and uses HTTP/HTTPS; otherwise, <c>false</c>.</returns>
    public static bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    #endregion

    #region Email Validation

    /// <summary>
    /// Validates that a string is a properly formatted email address.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns><c>true</c> if the email format is valid; otherwise, <c>false</c>.</returns>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex().IsMatch(email);
    }

    #endregion

    #region Color Validation

    /// <summary>
    /// Validates that a string is a valid hex color code (#RGB or #RRGGBB format).
    /// </summary>
    /// <param name="color">The color string to validate.</param>
    /// <returns><c>true</c> if the color is a valid hex format; otherwise, <c>false</c>.</returns>
    public static bool IsValidHexColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        return HexColorRegex().IsMatch(color);
    }

    /// <summary>
    /// Validates that a string is a recognized named color.
    /// </summary>
    /// <param name="color">The color name to validate.</param>
    /// <returns><c>true</c> if the color is a known named color; otherwise, <c>false</c>.</returns>
    public static bool IsValidNamedColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        var knownColors = new[]
        {
            "red", "green", "blue", "yellow", "orange", "purple", "pink",
            "cyan", "magenta", "white", "black", "gray", "grey", "silver",
            "gold", "navy", "teal", "maroon", "olive", "lime", "aqua",
            "fuchsia", "coral", "salmon", "brown"
        };

        return knownColors.Contains(color.ToLowerInvariant());
    }

    /// <summary>
    /// Validates that a string is either a valid hex color or a known named color.
    /// </summary>
    /// <param name="color">The color string to validate.</param>
    /// <returns><c>true</c> if the color is valid (hex or named); otherwise, <c>false</c>.</returns>
    public static bool IsValidColor(string color)
    {
        return IsValidHexColor(color) || IsValidNamedColor(color);
    }

    #endregion

    #region Image Validation

    /// <summary>
    /// Validates that a URL points to an image resource.
    /// </summary>
    /// <param name="url">The URL to validate.</param>
    /// <returns><c>true</c> if the URL is valid and contains an image extension; otherwise, <c>false</c>.</returns>
    public static bool IsValidImageUrl(string url)
    {
        if (!IsValidUrl(url))
            return false;

        var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".svg" };
        var lowerUrl = url.ToLowerInvariant();

        // Check if URL contains image extension (may have query params)
        return imageExtensions.Any(ext => lowerUrl.Contains(ext));
    }

    #endregion

    #region YouTube Validation

    /// <summary>
    /// Validates a YouTube URL or video ID.
    /// </summary>
    /// <param name="input">YouTube URL or 11-character video ID.</param>
    /// <returns><c>true</c> if the input is a valid YouTube reference; otherwise, <c>false</c>.</returns>
    public static bool IsValidYouTubeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        // Check for YouTube URL
        if (input.Contains("youtube.com") || input.Contains("youtu.be"))
            return IsValidUrl(input);

        // Check for video ID format (11 characters, alphanumeric with - and _)
        return YouTubeIdRegex().IsMatch(input);
    }

    /// <summary>
    /// Extracts the YouTube video ID from a URL or returns the input if already an ID.
    /// </summary>
    /// <param name="input">YouTube URL or video ID.</param>
    /// <returns>The 11-character video ID, or <c>null</c> if extraction fails.</returns>
    public static string? ExtractYouTubeId(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        // Already a video ID
        if (YouTubeIdRegex().IsMatch(input))
            return input;

        // Short URL format
        var shortMatch = YouTubeShortUrlRegex().Match(input);
        if (shortMatch.Success)
            return shortMatch.Groups[1].Value;

        // Standard URL format
        var standardMatch = YouTubeStandardUrlRegex().Match(input);
        if (standardMatch.Success)
            return standardMatch.Groups[1].Value;

        return null;
    }

    #endregion

    #region Size Validation

    /// <summary>
    /// Validates that a string represents a valid size value within the specified range.
    /// </summary>
    /// <param name="size">The size string to validate.</param>
    /// <param name="min">Minimum valid value (default: 1).</param>
    /// <param name="max">Maximum valid value (default: 7 for BBCode).</param>
    /// <returns><c>true</c> if the size is valid and within range; otherwise, <c>false</c>.</returns>
    public static bool IsValidSize(string size, int min = 1, int max = 7)
    {
        return int.TryParse(size, out var value) && value >= min && value <= max;
    }

    #endregion

    #region Sanitization

    /// <summary>
    /// Sanitizes input text by removing potentially dangerous characters and normalizing line endings.
    /// </summary>
    /// <param name="input">The input string to sanitize.</param>
    /// <returns>Sanitized string with null characters removed and line endings normalized to \n.</returns>
    public static string SanitizeInput(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Remove null characters
        input = input.Replace("\0", string.Empty, StringComparison.Ordinal);

        // Normalize line endings
        input = input.Replace("\r\n", "\n").Replace("\r", "\n");

        return input;
    }

    /// <summary>
    /// Normalizes a color value for consistent formatting.
    /// </summary>
    /// <param name="color">The color string to normalize.</param>
    /// <returns>Uppercase hex color or lowercase named color.</returns>
    /// <example>
    /// <code>
    /// NormalizeColor("#ff0000") // Returns "#FF0000"
    /// NormalizeColor("RED")     // Returns "red"
    /// </code>
    /// </example>
    public static string NormalizeColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return color;

        color = color.Trim();

        if (color.StartsWith('#'))
            return color.ToUpperInvariant();

        return color.ToLowerInvariant();
    }

    #endregion

    #region Generated Regex Patterns

    /// <summary>Email address validation pattern.</summary>
    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();

    /// <summary>Hex color validation pattern (#RGB or #RRGGBB).</summary>
    [GeneratedRegex(@"^#([0-9A-Fa-f]{3}|[0-9A-Fa-f]{6})$")]
    private static partial Regex HexColorRegex();

    /// <summary>YouTube video ID pattern (11 alphanumeric characters).</summary>
    [GeneratedRegex(@"^[a-zA-Z0-9_-]{11}$")]
    private static partial Regex YouTubeIdRegex();

    /// <summary>YouTube short URL extraction pattern (youtu.be/ID).</summary>
    [GeneratedRegex(@"youtu\.be/([a-zA-Z0-9_-]{11})")]
    private static partial Regex YouTubeShortUrlRegex();

    /// <summary>YouTube standard URL extraction pattern (?v=ID or &amp;v=ID).</summary>
    [GeneratedRegex(@"[?&]v=([a-zA-Z0-9_-]{11})")]
    private static partial Regex YouTubeStandardUrlRegex();

    #endregion
}
