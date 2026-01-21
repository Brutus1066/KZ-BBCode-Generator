using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Simple Machines Forum (SMF) BBCode generator
/// Standard BBCode with SMF-specific features
/// </summary>
public class SMFGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.SMF;

    // SMF uses superscript and subscript
    public string Superscript(string text) => $"[sup]{text}[/sup]";
    public string Subscript(string text) => $"[sub]{text}[/sub]";

    // SMF teletype (monospace)
    public string Teletype(string text) => $"[tt]{text}[/tt]";

    // SMF move (similar to marquee)
    public override string Marquee(string text) => $"[move]{text}[/move]";

    // SMF glow effect
    public string Glow(string text, string color) => $"[glow={color}]{text}[/glow]";

    // SMF shadow effect
    public string Shadow(string text, string color) => $"[shadow={color}]{text}[/shadow]";

    // SMF FTP link
    public string Ftp(string url, string? text = null)
    {
        var display = string.IsNullOrWhiteSpace(text) ? url : text;
        return $"[ftp={url}]{display}[/ftp]";
    }

    // SMF member link
    public override string Mention(string username)
    {
        var clean = username.TrimStart('@');
        return $"[member={clean}]{clean}[/member]";
    }
}
