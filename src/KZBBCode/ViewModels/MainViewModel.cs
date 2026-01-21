using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KZBBCode.Generators;
using KZBBCode.Models;
using KZBBCode.Services;

namespace KZBBCode.ViewModels;

/// <summary>
/// Main ViewModel for the KZ BBCode Generator application.
/// Implements MVVM pattern using CommunityToolkit.Mvvm for property change notifications and commands.
/// </summary>
/// <remarks>
/// <para>This ViewModel manages:</para>
/// <list type="bullet">
///   <item><description>User input text and generated output</description></item>
///   <item><description>Platform selection and corresponding generator</description></item>
///   <item><description>Theme preferences and auto-copy settings</description></item>
///   <item><description>All formatting commands (Bold, Italic, URL, etc.)</description></item>
/// </list>
/// </remarks>
public partial class MainViewModel : ObservableObject
{
    #region Observable Properties

    /// <summary>Gets or sets the user's input text to be formatted.</summary>
    [ObservableProperty]
    private string _inputText = string.Empty;

    /// <summary>Gets or sets the generated BBCode/Markdown output.</summary>
    [ObservableProperty]
    private string _outputText = string.Empty;

    /// <summary>Gets or sets the current status message displayed in the status bar.</summary>
    [ObservableProperty]
    private string _statusText = "Ready";

    /// <summary>Gets or sets the currently selected platform type.</summary>
    [ObservableProperty]
    private PlatformType _selectedPlatform = PlatformType.PhpBB;

    /// <summary>Gets or sets the currently selected theme name.</summary>
    [ObservableProperty]
    private string _selectedTheme = "Dark Mode";

    /// <summary>Gets or sets whether output is automatically copied to clipboard.</summary>
    [ObservableProperty]
    private bool _autoCopy = true;

    #endregion

    #region Private Fields

    private IBBCodeGen _generator;
    private readonly AppSettings _settings;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// Loads saved settings and configures the appropriate generator.
    /// </summary>
    public MainViewModel()
    {
        _settings = SettingsService.Load();
        _selectedTheme = _settings.ThemeName;
        _selectedPlatform = _settings.Platform;
        _autoCopy = _settings.AutoCopyToClipboard;
        _generator = GeneratorFactory.GetGenerator(_selectedPlatform);
    }

    #endregion

    #region Public Properties

    /// <summary>Gets the application settings instance.</summary>
    public AppSettings Settings => _settings;

    /// <summary>Gets all available platform names for the dropdown.</summary>
    public IEnumerable<string> PlatformNames => PlatformInfo.All.Select(p => p.Name);

    /// <summary>Gets all available theme names for the dropdown.</summary>
    public IEnumerable<string> ThemeNames => ThemeService.GetThemeNames();

    #endregion

    #region Property Changed Handlers

    /// <summary>Called when SelectedPlatform changes. Updates the generator instance.</summary>
    partial void OnSelectedPlatformChanged(PlatformType value)
    {
        _generator = GeneratorFactory.GetGenerator(value);
        _settings.PlatformName = PlatformInfo.GetByType(value).Name;
        UpdateStatus($"Platform: {PlatformInfo.GetByType(value).Name}");
    }

    /// <summary>Called when SelectedTheme changes. Saves the preference.</summary>
    partial void OnSelectedThemeChanged(string value)
    {
        _settings.ThemeName = value;
    }

    partial void OnAutoCopyChanged(bool value)
    {
        _settings.AutoCopyToClipboard = value;
    }

    public void SetPlatformByName(string name)
    {
        var platform = PlatformInfo.GetByName(name);
        SelectedPlatform = platform.Type;
    }

    // Text Formatting Commands
    [RelayCommand]
    private void ApplyBold()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Bold(InputText);
        CopyAndNotify("Bold");
    }

    [RelayCommand]
    private void ApplyItalic()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Italic(InputText);
        CopyAndNotify("Italic");
    }

    [RelayCommand]
    private void ApplyUnderline()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Underline(InputText);
        CopyAndNotify("Underline");
    }

    [RelayCommand]
    private void ApplyStrikethrough()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Strikethrough(InputText);
        CopyAndNotify("Strikethrough");
    }

    [RelayCommand]
    private void ApplyColor(string color)
    {
        if (string.IsNullOrEmpty(InputText) || string.IsNullOrEmpty(color)) return;
        OutputText = _generator.Color(InputText, color);
        CopyAndNotify("Color");
    }

    [RelayCommand]
    private void ApplySize(string size)
    {
        if (string.IsNullOrEmpty(InputText) || string.IsNullOrEmpty(size)) return;
        OutputText = _generator.Size(InputText, size);
        CopyAndNotify("Size");
    }

    [RelayCommand]
    private void ApplyFont(string fontName)
    {
        if (string.IsNullOrEmpty(InputText) || string.IsNullOrEmpty(fontName)) return;
        OutputText = _generator.Font(InputText, fontName);
        CopyAndNotify("Font");
    }

    // Link Commands
    [RelayCommand]
    private void ApplyUrl(string? displayText = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Url(InputText, displayText);
        CopyAndNotify("URL");
    }

    [RelayCommand]
    private void ApplyEmail(string? displayText = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Email(InputText, displayText);
        CopyAndNotify("Email");
    }

    [RelayCommand]
    private void ApplyMention()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Mention(InputText);
        CopyAndNotify("Mention");
    }

    // Media Commands
    [RelayCommand]
    private void ApplyImage(string? caption = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Image(InputText, caption);
        CopyAndNotify("Image");
    }

    [RelayCommand]
    private void ApplyVideo()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Video(InputText);
        CopyAndNotify("Video");
    }

    [RelayCommand]
    private void ApplyYouTube()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.YouTube(InputText);
        CopyAndNotify("YouTube");
    }

    // Structure Commands
    [RelayCommand]
    private void ApplyQuote(string? author = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Quote(InputText, author);
        CopyAndNotify("Quote");
    }

    [RelayCommand]
    private void ApplyCode(string? language = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Code(InputText, language);
        CopyAndNotify("Code");
    }

    [RelayCommand]
    private void ApplySpoiler(string? title = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Spoiler(InputText, title);
        CopyAndNotify("Spoiler");
    }

    [RelayCommand]
    private void ApplyList(ListType type = ListType.Bullet)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        var items = InputText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        OutputText = _generator.List(items, type);
        CopyAndNotify("List");
    }

    [RelayCommand]
    private void ApplyAlign(TextAlignment alignment)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Align(InputText, alignment);
        CopyAndNotify($"Align {alignment}");
    }

    [RelayCommand]
    private void ApplyHeader(HeaderLevel level)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Header(InputText, level);
        CopyAndNotify($"Header {(int)level}");
    }

    [RelayCommand]
    private void ApplyHorizontalRule()
    {
        OutputText = _generator.HorizontalRule();
        CopyAndNotify("Horizontal Rule");
    }

    // Special Commands
    [RelayCommand]
    private void ApplyProgressBar((int Percent, string? Label) args)
    {
        OutputText = _generator.ProgressBar(args.Percent, args.Label);
        CopyAndNotify("Progress Bar");
    }

    [RelayCommand]
    private void ApplyMarquee()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Marquee(InputText);
        CopyAndNotify("Marquee");
    }

    [RelayCommand]
    private void ApplyHide(string? buttonText = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.Hide(InputText, buttonText);
        CopyAndNotify("Hide");
    }

    // Table generation
    public void ApplyTable(string[,] cells, bool hasHeader = true)
    {
        OutputText = _generator.Table(cells, hasHeader);
        CopyAndNotify("Table");
    }

    // Combined formatting
    public void ApplyFormatted(bool bold, bool italic, bool underline, bool strikethrough,
        string? color = null, string? size = null)
    {
        if (string.IsNullOrEmpty(InputText)) return;
        OutputText = _generator.FormatText(InputText, bold, italic, underline, strikethrough, color, size);
        CopyAndNotify("Formatted");
    }

    // Batch operations
    [RelayCommand]
    private void BatchUrls()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        var urls = InputText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        OutputText = string.Join("\n", urls.Select(u => _generator.Url(u.Trim())));
        CopyAndNotify($"Batch URLs ({urls.Length})");
    }

    [RelayCommand]
    private void BatchImages()
    {
        if (string.IsNullOrEmpty(InputText)) return;
        var urls = InputText.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        OutputText = string.Join("\n\n", urls.Select(u => _generator.Image(u.Trim())));
        CopyAndNotify($"Batch Images ({urls.Length})");
    }

    // History
    [RelayCommand]
    private void ClearInput()
    {
        InputText = string.Empty;
        OutputText = string.Empty;
        UpdateStatus("Cleared");
    }

    [RelayCommand]
    private void CopyOutput()
    {
        if (string.IsNullOrEmpty(OutputText)) return;
        CopyToClipboard(OutputText);
        UpdateStatus("Copied to clipboard");
    }

    // Helpers
    private void CopyAndNotify(string action)
    {
        if (AutoCopy && !string.IsNullOrEmpty(OutputText))
        {
            CopyToClipboard(OutputText);
            SettingsService.AddToHistory(_settings, OutputText);
        }
        UpdateStatus($"{action} applied" + (AutoCopy ? " (copied)" : ""));
    }

    private void CopyToClipboard(string text)
    {
        try
        {
            Clipboard.SetText(text);
        }
        catch
        {
            // Clipboard access can fail
        }
    }

    private void UpdateStatus(string message)
    {
        var platform = PlatformInfo.GetByType(SelectedPlatform);
        StatusText = $"{message} | Platform: {platform.Name} | Theme: {SelectedTheme}";
    }

    public void SaveSettings()
    {
        SettingsService.Save(_settings);
    }

    #endregion
}
