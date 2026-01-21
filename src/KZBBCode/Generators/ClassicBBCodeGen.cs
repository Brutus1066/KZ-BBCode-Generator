using KZBBCode.Models;

namespace KZBBCode.Generators;

/// <summary>
/// Classic BBCode generator - Traditional forum standard with extended tags
/// </summary>
public class ClassicBBCodeGen : BBCodeBase
{
    public override PlatformType Platform => PlatformType.ClassicBBCode;

    // Pre-formatted text for ASCII art
    public string Pre(string text) => $"[pre]{text}[/pre]";

    // NFO style formatting
    public string Nfo(string text) => $"[nfo]{text}[/nfo]";
}
