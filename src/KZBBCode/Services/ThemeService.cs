namespace KZBBCode.Services;

/// <summary>
/// Application theme definition containing all UI colors.
/// </summary>
/// <remarks>
/// <para>Themes are immutable records that define the complete color palette for the application.</para>
/// <para>Each theme specifies colors for backgrounds, foregrounds, accents, buttons, inputs, and outputs.</para>
/// </remarks>
/// <param name="Name">Display name shown in the theme selector dropdown.</param>
/// <param name="Background">Primary window and panel background color.</param>
/// <param name="Foreground">Primary text color for labels and general content.</param>
/// <param name="Accent">Accent color used for highlights, links, and emphasis.</param>
/// <param name="ButtonBackground">Background color for all buttons and toolbar items.</param>
/// <param name="ButtonForeground">Text color for button labels and toolbar icons.</param>
/// <param name="InputBackground">Background color for text input fields.</param>
/// <param name="InputForeground">Text color for user input in text fields.</param>
/// <param name="OutputBackground">Background color for the generated code output panel.</param>
/// <param name="OutputForeground">Text color for the generated code output.</param>
/// <param name="BorderColor">Color used for control borders and separators.</param>
/// <param name="HighlightColor">Color used for selection highlights and focus indicators.</param>
public record Theme(
    string Name,
    Color Background,
    Color Foreground,
    Color Accent,
    Color ButtonBackground,
    Color ButtonForeground,
    Color InputBackground,
    Color InputForeground,
    Color OutputBackground,
    Color OutputForeground,
    Color BorderColor,
    Color HighlightColor
);

/// <summary>
/// Provides theme definitions and application services for the UI.
/// </summary>
/// <remarks>
/// <para>Contains 16 carefully crafted visual themes ranging from professional dark modes
/// to vibrant creative palettes and accessibility-focused options.</para>
/// <para>Themes are applied recursively to all controls using the <see cref="ApplyTheme"/> method.</para>
/// </remarks>
/// <example>
/// <code>
/// // Get a theme by name and apply it
/// var theme = ThemeService.GetByName("Dracula");
/// ThemeService.ApplyTheme(this, theme);
/// </code>
/// </example>
public static class ThemeService
{
    public static Theme DarkMode => new(
        "Dark Mode",
        Color.FromArgb(18, 18, 18),
        Color.FromArgb(224, 224, 224),
        Color.FromArgb(100, 149, 237),
        Color.FromArgb(45, 45, 45),
        Color.FromArgb(224, 224, 224),
        Color.FromArgb(30, 30, 30),
        Color.FromArgb(200, 200, 200),
        Color.FromArgb(25, 25, 25),
        Color.FromArgb(180, 180, 180),
        Color.FromArgb(60, 60, 60),
        Color.FromArgb(100, 149, 237)
    );

    public static Theme Matrix => new(
        "Matrix",
        Color.FromArgb(0, 10, 0),
        Color.FromArgb(0, 255, 65),
        Color.FromArgb(0, 200, 50),
        Color.FromArgb(0, 30, 0),
        Color.FromArgb(0, 255, 65),
        Color.FromArgb(0, 20, 0),
        Color.FromArgb(0, 220, 55),
        Color.FromArgb(0, 15, 0),
        Color.FromArgb(0, 180, 45),
        Color.FromArgb(0, 80, 20),
        Color.FromArgb(0, 255, 100)
    );

    public static Theme Neon => new(
        "Neon",
        Color.FromArgb(15, 15, 35),
        Color.FromArgb(255, 0, 255),
        Color.FromArgb(0, 255, 255),
        Color.FromArgb(30, 30, 60),
        Color.FromArgb(255, 100, 255),
        Color.FromArgb(20, 20, 45),
        Color.FromArgb(200, 100, 255),
        Color.FromArgb(18, 18, 40),
        Color.FromArgb(0, 200, 200),
        Color.FromArgb(100, 0, 150),
        Color.FromArgb(255, 255, 0)
    );

    public static Theme Cyberpunk => new(
        "Cyberpunk",
        Color.FromArgb(20, 10, 25),
        Color.FromArgb(255, 50, 100),
        Color.FromArgb(255, 215, 0),
        Color.FromArgb(50, 20, 40),
        Color.FromArgb(255, 80, 120),
        Color.FromArgb(35, 15, 30),
        Color.FromArgb(255, 100, 130),
        Color.FromArgb(30, 12, 28),
        Color.FromArgb(255, 200, 50),
        Color.FromArgb(150, 50, 80),
        Color.FromArgb(0, 255, 255)
    );

    public static Theme Ocean => new(
        "Ocean",
        Color.FromArgb(10, 25, 40),
        Color.FromArgb(100, 200, 255),
        Color.FromArgb(50, 150, 200),
        Color.FromArgb(20, 50, 80),
        Color.FromArgb(150, 220, 255),
        Color.FromArgb(15, 35, 55),
        Color.FromArgb(120, 190, 230),
        Color.FromArgb(12, 30, 48),
        Color.FromArgb(80, 180, 220),
        Color.FromArgb(40, 100, 140),
        Color.FromArgb(0, 255, 200)
    );

    public static Theme Synthwave => new(
        "Synthwave",
        Color.FromArgb(25, 10, 40),
        Color.FromArgb(255, 110, 199),
        Color.FromArgb(255, 200, 100),
        Color.FromArgb(60, 20, 80),
        Color.FromArgb(255, 150, 220),
        Color.FromArgb(40, 15, 60),
        Color.FromArgb(255, 130, 210),
        Color.FromArgb(35, 12, 55),
        Color.FromArgb(100, 200, 255),
        Color.FromArgb(120, 40, 150),
        Color.FromArgb(255, 255, 100)
    );

    public static Theme LightMode => new(
        "Light Mode",
        Color.FromArgb(250, 250, 250),
        Color.FromArgb(30, 30, 30),
        Color.FromArgb(0, 120, 212),
        Color.FromArgb(230, 230, 230),
        Color.FromArgb(30, 30, 30),
        Color.FromArgb(255, 255, 255),
        Color.FromArgb(20, 20, 20),
        Color.FromArgb(245, 245, 245),
        Color.FromArgb(40, 40, 40),
        Color.FromArgb(200, 200, 200),
        Color.FromArgb(0, 120, 212)
    );

    public static Theme SolarizedDark => new(
        "Solarized Dark",
        Color.FromArgb(0, 43, 54),
        Color.FromArgb(131, 148, 150),
        Color.FromArgb(38, 139, 210),
        Color.FromArgb(7, 54, 66),
        Color.FromArgb(147, 161, 161),
        Color.FromArgb(0, 43, 54),
        Color.FromArgb(131, 148, 150),
        Color.FromArgb(7, 54, 66),
        Color.FromArgb(147, 161, 161),
        Color.FromArgb(88, 110, 117),
        Color.FromArgb(181, 137, 0)
    );

    public static Theme SolarizedLight => new(
        "Solarized Light",
        Color.FromArgb(253, 246, 227),
        Color.FromArgb(101, 123, 131),
        Color.FromArgb(38, 139, 210),
        Color.FromArgb(238, 232, 213),
        Color.FromArgb(88, 110, 117),
        Color.FromArgb(253, 246, 227),
        Color.FromArgb(101, 123, 131),
        Color.FromArgb(238, 232, 213),
        Color.FromArgb(88, 110, 117),
        Color.FromArgb(147, 161, 161),
        Color.FromArgb(181, 137, 0)
    );

    public static Theme Monokai => new(
        "Monokai",
        Color.FromArgb(39, 40, 34),
        Color.FromArgb(248, 248, 242),
        Color.FromArgb(166, 226, 46),
        Color.FromArgb(60, 61, 54),
        Color.FromArgb(248, 248, 242),
        Color.FromArgb(45, 46, 40),
        Color.FromArgb(230, 219, 116),
        Color.FromArgb(50, 51, 45),
        Color.FromArgb(249, 38, 114),
        Color.FromArgb(117, 113, 94),
        Color.FromArgb(102, 217, 239)
    );

    public static Theme Nord => new(
        "Nord",
        Color.FromArgb(46, 52, 64),
        Color.FromArgb(216, 222, 233),
        Color.FromArgb(136, 192, 208),
        Color.FromArgb(59, 66, 82),
        Color.FromArgb(229, 233, 240),
        Color.FromArgb(59, 66, 82),
        Color.FromArgb(216, 222, 233),
        Color.FromArgb(67, 76, 94),
        Color.FromArgb(229, 233, 240),
        Color.FromArgb(76, 86, 106),
        Color.FromArgb(163, 190, 140)
    );

    public static Theme HighContrast => new(
        "High Contrast",
        Color.FromArgb(0, 0, 0),
        Color.FromArgb(255, 255, 255),
        Color.FromArgb(255, 255, 0),
        Color.FromArgb(0, 0, 0),
        Color.FromArgb(255, 255, 255),
        Color.FromArgb(0, 0, 0),
        Color.FromArgb(255, 255, 255),
        Color.FromArgb(0, 0, 0),
        Color.FromArgb(0, 255, 255),
        Color.FromArgb(255, 255, 255),
        Color.FromArgb(255, 0, 255)
    );

    /// <summary>
    /// Dracula theme - Popular dark theme with purple accents.
    /// </summary>
    public static Theme Dracula => new(
        "Dracula",
        Color.FromArgb(40, 42, 54),      // Background
        Color.FromArgb(248, 248, 242),   // Foreground
        Color.FromArgb(189, 147, 249),   // Purple accent
        Color.FromArgb(68, 71, 90),      // Button background
        Color.FromArgb(248, 248, 242),   // Button foreground
        Color.FromArgb(33, 34, 44),      // Input background
        Color.FromArgb(248, 248, 242),   // Input foreground
        Color.FromArgb(40, 42, 54),      // Output background
        Color.FromArgb(80, 250, 123),    // Green output
        Color.FromArgb(98, 114, 164),    // Border
        Color.FromArgb(255, 121, 198)    // Pink highlight
    );

    /// <summary>
    /// Gruvbox Dark theme - Retro groove color scheme.
    /// </summary>
    public static Theme GruvboxDark => new(
        "Gruvbox Dark",
        Color.FromArgb(40, 40, 40),      // bg0
        Color.FromArgb(235, 219, 178),   // fg1
        Color.FromArgb(250, 189, 47),    // Yellow accent
        Color.FromArgb(60, 56, 54),      // bg1
        Color.FromArgb(235, 219, 178),   // fg1
        Color.FromArgb(50, 48, 47),      // bg0_h
        Color.FromArgb(213, 196, 161),   // fg2
        Color.FromArgb(40, 40, 40),      // bg0
        Color.FromArgb(184, 187, 38),    // Green output
        Color.FromArgb(102, 92, 84),     // gray
        Color.FromArgb(251, 73, 52)      // Red highlight
    );

    /// <summary>
    /// Tokyo Night theme - Popular VS Code theme with rich blues.
    /// </summary>
    public static Theme TokyoNight => new(
        "Tokyo Night",
        Color.FromArgb(26, 27, 38),      // Background
        Color.FromArgb(169, 177, 214),   // Foreground
        Color.FromArgb(122, 162, 247),   // Blue accent
        Color.FromArgb(36, 40, 59),      // Button background
        Color.FromArgb(192, 202, 245),   // Button foreground
        Color.FromArgb(30, 31, 42),      // Input background
        Color.FromArgb(169, 177, 214),   // Input foreground
        Color.FromArgb(26, 27, 38),      // Output background
        Color.FromArgb(158, 206, 106),   // Green output
        Color.FromArgb(61, 89, 161),     // Border
        Color.FromArgb(255, 117, 127)    // Pink highlight
    );

    /// <summary>
    /// Catppuccin Mocha theme - Soothing pastel theme for productivity.
    /// </summary>
    public static Theme CatppuccinMocha => new(
        "Catppuccin Mocha",
        Color.FromArgb(30, 30, 46),      // Base
        Color.FromArgb(205, 214, 244),   // Text
        Color.FromArgb(137, 180, 250),   // Blue accent
        Color.FromArgb(49, 50, 68),      // Surface0
        Color.FromArgb(205, 214, 244),   // Text
        Color.FromArgb(36, 39, 58),      // Mantle
        Color.FromArgb(186, 194, 222),   // Subtext1
        Color.FromArgb(30, 30, 46),      // Base
        Color.FromArgb(166, 227, 161),   // Green output
        Color.FromArgb(69, 71, 90),      // Surface1
        Color.FromArgb(245, 194, 231)    // Pink highlight
    );

    /// <summary>
    /// All available themes in display order.
    /// </summary>
    public static IReadOnlyList<Theme> AllThemes => new[]
    {
        DarkMode,
        Dracula,
        TokyoNight,
        CatppuccinMocha,
        GruvboxDark,
        Matrix,
        Neon,
        Cyberpunk,
        Ocean,
        Synthwave,
        LightMode,
        SolarizedDark,
        SolarizedLight,
        Monokai,
        Nord,
        HighContrast
    };

    /// <summary>
    /// Get theme by name
    /// </summary>
    public static Theme GetByName(string name) =>
        AllThemes.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? DarkMode;

    /// <summary>
    /// Get all theme names
    /// </summary>
    public static IEnumerable<string> GetThemeNames() => AllThemes.Select(t => t.Name);

    /// <summary>
    /// Apply theme to a form and its controls recursively
    /// </summary>
    public static void ApplyTheme(Control control, Theme theme)
    {
        control.BackColor = theme.Background;
        control.ForeColor = theme.Foreground;

        foreach (Control child in control.Controls)
        {
            ApplyThemeToControl(child, theme);
        }
    }

    private static void ApplyThemeToControl(Control control, Theme theme)
    {
        switch (control)
        {
            case Button btn:
                btn.BackColor = theme.ButtonBackground;
                btn.ForeColor = theme.ButtonForeground;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = theme.BorderColor;
                break;

            case TextBox txt:
                txt.BackColor = theme.InputBackground;
                txt.ForeColor = theme.InputForeground;
                txt.BorderStyle = BorderStyle.FixedSingle;
                break;

            case RichTextBox rtb:
                rtb.BackColor = theme.InputBackground;
                rtb.ForeColor = theme.InputForeground;
                rtb.BorderStyle = BorderStyle.None;
                break;

            case ComboBox cmb:
                cmb.BackColor = theme.InputBackground;
                cmb.ForeColor = theme.InputForeground;
                cmb.FlatStyle = FlatStyle.Flat;
                break;

            case Label lbl:
                lbl.BackColor = Color.Transparent;
                lbl.ForeColor = theme.Foreground;
                break;

            case Panel pnl:
                pnl.BackColor = theme.Background;
                break;

            case StatusStrip status:
                status.BackColor = theme.ButtonBackground;
                status.ForeColor = theme.Foreground;
                foreach (ToolStripItem item in status.Items)
                {
                    item.BackColor = theme.ButtonBackground;
                    item.ForeColor = theme.Foreground;
                }
                break;

            case MenuStrip menu:
                menu.BackColor = theme.ButtonBackground;
                menu.ForeColor = theme.Foreground;
                foreach (ToolStripItem item in menu.Items)
                {
                    item.BackColor = theme.ButtonBackground;
                    item.ForeColor = theme.ButtonForeground;
                }
                break;

            case ToolStrip toolbar:
                toolbar.BackColor = theme.ButtonBackground;
                toolbar.ForeColor = theme.Foreground;
                foreach (ToolStripItem item in toolbar.Items)
                {
                    item.BackColor = theme.ButtonBackground;
                    item.ForeColor = theme.ButtonForeground;
                }
                break;

            default:
                control.BackColor = theme.Background;
                control.ForeColor = theme.Foreground;
                break;
        }

        foreach (Control child in control.Controls)
        {
            ApplyThemeToControl(child, theme);
        }
    }
}
