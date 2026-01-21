using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Factory for creating and managing platform-specific BBCode and Markdown generators.
/// </summary>
/// <remarks>
/// <para>This static factory class provides centralized access to all platform generators.
/// It maintains a dictionary of pre-instantiated generators for efficient reuse.</para>
/// 
/// <para><b>Supported Platforms:</b></para>
/// <list type="bullet">
///   <item><description><b>BBCode Forums:</b> phpBB, vBulletin, MyBB, Classic BBCode, SMF, IPB, XenForo</description></item>
///   <item><description><b>Markdown Platforms:</b> Discourse, Discord</description></item>
///   <item><description><b>Custom Formats:</b> Slack (mrkdwn)</description></item>
/// </list>
/// 
/// <para><b>Usage Pattern:</b> Call <see cref="GetGenerator(PlatformType)"/> to obtain
/// a generator instance, then use its methods to format content.</para>
/// </remarks>
/// <example>
/// <code>
/// // Get generator by platform type
/// var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
/// var formatted = gen.Bold("Hello") + " " + gen.Italic("World");
/// 
/// // Get generator by platform name
/// var phpGen = GeneratorFactory.GetGenerator("phpBB");
/// </code>
/// </example>
public static class GeneratorFactory
{
    /// <summary>
    /// Dictionary of all available generators, keyed by platform type.
    /// </summary>
    private static readonly Dictionary<PlatformType, IBBCodeGen> _generators = new()
    {
        [PlatformType.PhpBB] = new PhpBBGen(),
        [PlatformType.VBulletin] = new VBulletinGen(),
        [PlatformType.MyBB] = new MyBBGen(),
        [PlatformType.ClassicBBCode] = new ClassicBBCodeGen(),
        [PlatformType.SMF] = new SMFGen(),
        [PlatformType.IPB] = new IPBGen(),
        [PlatformType.XenForo] = new XenForoGen(),
        [PlatformType.Discourse] = new DiscourseGen(),
        [PlatformType.Discord] = new DiscordGen(),
        [PlatformType.Slack] = new SlackGen()
    };

    /// <summary>
    /// Gets the generator for a specific platform type.
    /// </summary>
    /// <param name="platform">The target platform type.</param>
    /// <returns>The appropriate generator, or phpBB generator as fallback.</returns>
    public static IBBCodeGen GetGenerator(PlatformType platform)
    {
        return _generators.TryGetValue(platform, out var gen) ? gen : _generators[PlatformType.PhpBB];
    }

    /// <summary>
    /// Gets the generator by platform display name.
    /// </summary>
    /// <param name="platformName">The platform name (e.g., "phpBB", "Discord").</param>
    /// <returns>The appropriate generator, or phpBB generator as fallback.</returns>
    public static IBBCodeGen GetGenerator(string platformName)
    {
        var platform = PlatformInfo.GetByName(platformName).Type;
        return GetGenerator(platform);
    }

    /// <summary>
    /// Gets all available generators.
    /// </summary>
    /// <returns>Collection of all registered generators.</returns>
    public static IEnumerable<IBBCodeGen> GetAllGenerators() => _generators.Values;

    /// <summary>
    /// Determines if a platform uses BBCode format.
    /// </summary>
    /// <param name="platform">The platform to check.</param>
    /// <returns><c>true</c> if the platform uses BBCode; otherwise, <c>false</c>.</returns>
    public static bool UsesBBCode(PlatformType platform)
    {
        var info = PlatformInfo.GetByType(platform);
        return info.Format == FormatType.BBCode;
    }

    /// <summary>
    /// Determines if a platform uses Markdown format.
    /// </summary>
    /// <param name="platform">The platform to check.</param>
    /// <returns><c>true</c> if the platform uses Markdown or Slack mrkdwn; otherwise, <c>false</c>.</returns>
    public static bool UsesMarkdown(PlatformType platform)
    {
        var info = PlatformInfo.GetByType(platform);
        return info.Format == FormatType.Markdown || info.Format == FormatType.SlackMarkdown;
    }
}
