using KZBBCode.Generators;
using KZBBCode.Models;
using Xunit;

namespace KZBBCode.Tests;

public class GeneratorTests
{
    [Fact]
    public void PhpBB_Bold_GeneratesCorrectBBCode()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Bold("test");
        Assert.Equal("[b]test[/b]", result);
    }

    [Fact]
    public void PhpBB_Italic_GeneratesCorrectBBCode()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Italic("test");
        Assert.Equal("[i]test[/i]", result);
    }

    [Fact]
    public void PhpBB_Url_GeneratesCorrectBBCode()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Url("https://example.com", "Click Here");
        Assert.Equal("[url=https://example.com]Click Here[/url]", result);
    }

    [Fact]
    public void PhpBB_Url_WithoutText_UsesUrlAsText()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Url("https://example.com");
        Assert.Equal("[url=https://example.com]https://example.com[/url]", result);
    }

    [Fact]
    public void Discord_Bold_GeneratesMarkdown()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
        var result = gen.Bold("test");
        Assert.Equal("**test**", result);
    }

    [Fact]
    public void Discord_Italic_GeneratesMarkdown()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
        var result = gen.Italic("test");
        Assert.Equal("*test*", result);
    }

    [Fact]
    public void Discord_Underline_GeneratesMarkdown()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
        var result = gen.Underline("test");
        Assert.Equal("__test__", result);
    }

    [Fact]
    public void Discord_Strikethrough_GeneratesMarkdown()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
        var result = gen.Strikethrough("test");
        Assert.Equal("~~test~~", result);
    }

    [Fact]
    public void Discord_Spoiler_GeneratesMarkdown()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Discord);
        var result = gen.Spoiler("secret");
        Assert.Equal("||secret||", result);
    }

    [Fact]
    public void Slack_Bold_UsesSingleAsterisk()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Slack);
        var result = gen.Bold("test");
        Assert.Equal("*test*", result);
    }

    [Fact]
    public void Slack_Italic_UsesUnderscore()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Slack);
        var result = gen.Italic("test");
        Assert.Equal("_test_", result);
    }

    [Fact]
    public void Slack_Strikethrough_UsesTilde()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.Slack);
        var result = gen.Strikethrough("test");
        Assert.Equal("~test~", result);
    }

    [Fact]
    public void PhpBB_Quote_WithAuthor_IncludesAuthor()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Quote("Hello world", "John");
        Assert.Equal("[quote=John]Hello world[/quote]", result);
    }

    [Fact]
    public void PhpBB_Code_WithLanguage_IncludesLanguage()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.Code("print('hi')", "python");
        Assert.Contains("[code=python]", result);
    }

    [Fact]
    public void PhpBB_List_Bullet_GeneratesCorrectFormat()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.List(new[] { "Item 1", "Item 2" }, ListType.Bullet);
        Assert.Contains("[list]", result);
        Assert.Contains("[*]Item 1", result);
        Assert.Contains("[*]Item 2", result);
        Assert.Contains("[/list]", result);
    }

    [Fact]
    public void PhpBB_List_Numbered_GeneratesCorrectFormat()
    {
        var gen = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        var result = gen.List(new[] { "First", "Second" }, ListType.Numbered);
        Assert.Contains("[list=1]", result);
    }

    [Fact]
    public void GeneratorFactory_GetGenerator_ReturnsCorrectType()
    {
        var phpbb = GeneratorFactory.GetGenerator(PlatformType.PhpBB);
        Assert.IsType<PhpBBGen>(phpbb);

        var discord = GeneratorFactory.GetGenerator(PlatformType.Discord);
        Assert.IsType<DiscordGen>(discord);

        var slack = GeneratorFactory.GetGenerator(PlatformType.Slack);
        Assert.IsType<SlackGen>(slack);
    }

    [Fact]
    public void GeneratorFactory_UsesBBCode_ReturnsCorrectly()
    {
        Assert.True(GeneratorFactory.UsesBBCode(PlatformType.PhpBB));
        Assert.True(GeneratorFactory.UsesBBCode(PlatformType.VBulletin));
        Assert.False(GeneratorFactory.UsesBBCode(PlatformType.Discord));
        Assert.False(GeneratorFactory.UsesBBCode(PlatformType.Slack));
    }

    [Fact]
    public void GeneratorFactory_UsesMarkdown_ReturnsCorrectly()
    {
        Assert.False(GeneratorFactory.UsesMarkdown(PlatformType.PhpBB));
        Assert.True(GeneratorFactory.UsesMarkdown(PlatformType.Discord));
        Assert.True(GeneratorFactory.UsesMarkdown(PlatformType.Slack));
        Assert.True(GeneratorFactory.UsesMarkdown(PlatformType.Discourse));
    }
}
