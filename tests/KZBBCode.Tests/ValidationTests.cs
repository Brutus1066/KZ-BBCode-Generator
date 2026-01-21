using KZBBCode.Helpers;
using Xunit;

namespace KZBBCode.Tests;

public class ValidationTests
{
    [Theory]
    [InlineData("https://example.com", true)]
    [InlineData("http://example.com", true)]
    [InlineData("https://sub.example.com/path?query=1", true)]
    [InlineData("ftp://example.com", false)]
    [InlineData("not-a-url", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidUrl_ValidatesCorrectly(string? url, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidUrl(url!));
    }

    [Theory]
    [InlineData("test@example.com", true)]
    [InlineData("user.name@domain.co.uk", true)]
    [InlineData("invalid-email", false)]
    [InlineData("@nodomain.com", false)]
    [InlineData("", false)]
    public void IsValidEmail_ValidatesCorrectly(string email, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidEmail(email));
    }

    [Theory]
    [InlineData("#FF0000", true)]
    [InlineData("#fff", true)]
    [InlineData("#AABBCC", true)]
    [InlineData("FF0000", false)]
    [InlineData("#GGGGGG", false)]
    [InlineData("#12345", false)]
    [InlineData("", false)]
    public void IsValidHexColor_ValidatesCorrectly(string color, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidHexColor(color));
    }

    [Theory]
    [InlineData("red", true)]
    [InlineData("blue", true)]
    [InlineData("GREEN", true)]
    [InlineData("invalid", false)]
    [InlineData("", false)]
    public void IsValidNamedColor_ValidatesCorrectly(string color, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidNamedColor(color));
    }

    [Theory]
    [InlineData("#FF0000", true)]
    [InlineData("red", true)]
    [InlineData("invalid", false)]
    public void IsValidColor_ValidatesBoth(string color, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidColor(color));
    }

    [Theory]
    [InlineData("https://example.com/image.jpg", true)]
    [InlineData("https://example.com/image.PNG", true)]
    [InlineData("https://example.com/image.gif?size=large", true)]
    [InlineData("https://example.com/page.html", false)]
    [InlineData("not-a-url", false)]
    public void IsValidImageUrl_ValidatesCorrectly(string url, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidImageUrl(url));
    }

    [Theory]
    [InlineData("dQw4w9WgXcQ", true)]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ", true)]
    [InlineData("https://youtu.be/dQw4w9WgXcQ", true)]
    [InlineData("invalid", false)]
    [InlineData("", false)]
    public void IsValidYouTubeInput_ValidatesCorrectly(string input, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidYouTubeInput(input));
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://youtu.be/dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    [InlineData("https://www.youtube.com/watch?v=dQw4w9WgXcQ&t=120", "dQw4w9WgXcQ")]
    [InlineData("dQw4w9WgXcQ", "dQw4w9WgXcQ")]
    public void ExtractYouTubeId_ExtractsCorrectly(string input, string expected)
    {
        Assert.Equal(expected, Validation.ExtractYouTubeId(input));
    }

    [Theory]
    [InlineData("1", 1, 7, true)]
    [InlineData("7", 1, 7, true)]
    [InlineData("0", 1, 7, false)]
    [InlineData("8", 1, 7, false)]
    [InlineData("abc", 1, 7, false)]
    public void IsValidSize_ValidatesCorrectly(string size, int min, int max, bool expected)
    {
        Assert.Equal(expected, Validation.IsValidSize(size, min, max));
    }

    [Fact]
    public void SanitizeInput_RemovesNullCharacters()
    {
        var input = "Hello\0World";
        var result = Validation.SanitizeInput(input);
        Assert.False(result.Contains('\0'), $"Result should not contain null char. Length: {result.Length}, Expected: 10");
        Assert.Equal("HelloWorld", result);
    }

    [Fact]
    public void SanitizeInput_NormalizesLineEndings()
    {
        var input = "Line1\r\nLine2\rLine3";
        var result = Validation.SanitizeInput(input);
        Assert.DoesNotContain("\r", result);
        Assert.Equal("Line1\nLine2\nLine3", result);
    }

    [Theory]
    [InlineData("#ff0000", "#FF0000")]
    [InlineData("RED", "red")]
    [InlineData("Blue", "blue")]
    public void NormalizeColor_NormalizesCorrectly(string input, string expected)
    {
        Assert.Equal(expected, Validation.NormalizeColor(input));
    }
}
