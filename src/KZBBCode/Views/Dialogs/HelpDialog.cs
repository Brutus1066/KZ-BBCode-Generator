namespace KZBBCode.Views.Dialogs;

/// <summary>
/// Help dialog with categorized help content
/// </summary>
public class HelpDialog : Form
{
    private ListBox _sectionList = null!;
    private RichTextBox _contentBox = null!;

    public HelpDialog()
    {
        Text = "KZ BBCode Generator - Help";
        Size = new Size(800, 600);
        MinimumSize = new Size(700, 500);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        var splitContainer = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 200,
            FixedPanel = FixedPanel.Panel1
        };

        // Section list
        _sectionList = new ListBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F),
            BorderStyle = BorderStyle.None
        };

        foreach (var section in HelpContent.Sections.Keys)
        {
            _sectionList.Items.Add(section);
        }

        _sectionList.SelectedIndexChanged += OnSectionSelected;
        splitContainer.Panel1.Controls.Add(_sectionList);

        // Content display
        _contentBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 10F),
            ReadOnly = true,
            WordWrap = true,
            BorderStyle = BorderStyle.None
        };
        splitContainer.Panel2.Controls.Add(_contentBox);

        // Close button panel
        var buttonPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 45
        };

        var closeBtn = new Button
        {
            Text = "Close",
            DialogResult = DialogResult.Cancel,
            Width = 80,
            Height = 30,
            Anchor = AnchorStyles.Right | AnchorStyles.Bottom
        };
        closeBtn.Location = new Point(buttonPanel.Width - closeBtn.Width - 15, 8);
        buttonPanel.Resize += (s, e) => closeBtn.Location = new Point(buttonPanel.Width - closeBtn.Width - 15, 8);

        buttonPanel.Controls.Add(closeBtn);
        CancelButton = closeBtn;

        Controls.Add(splitContainer);
        Controls.Add(buttonPanel);

        // Select first section
        if (_sectionList.Items.Count > 0)
            _sectionList.SelectedIndex = 0;
    }

    private void OnSectionSelected(object? sender, EventArgs e)
    {
        if (_sectionList.SelectedItem is string section && HelpContent.Sections.TryGetValue(section, out var content))
        {
            _contentBox.Text = content;
        }
    }
}

/// <summary>
/// Help content sections
/// </summary>
public static class HelpContent
{
    public static readonly Dictionary<string, string> Sections = new()
    {
        ["1. Quick Start"] = @"
KZ BBCODE GENERATOR v3.0
═══════════════════════════════════════════

GETTING STARTED

  1. SELECT PLATFORM
     Choose your target forum from the dropdown.
     Each platform uses different formatting syntax.

  2. ENTER TEXT
     Type or paste content in the Input area.
     For lists, put each item on a new line.

  3. APPLY FORMATTING
     Click toolbar buttons to format your text.
     Output appears instantly in the right panel.

  4. COPY & PASTE
     Code auto-copies to clipboard when enabled.
     Paste directly into your forum post.

───────────────────────────────────────────
KEYBOARD SHORTCUTS
───────────────────────────────────────────
  Ctrl+B    Bold          Ctrl+I    Italic
  Ctrl+U    Underline     Ctrl+K    URL
  F1        Help          Esc       Close
",

        ["2. Supported Platforms"] = @"
SUPPORTED PLATFORMS
═══════════════════════════════════════════

LEGACY FORUMS (BBCode)
  • phpBB         Classic open-source forum
  • vBulletin     Commercial forum software
  • MyBB          Free community forum
  • Classic BBCode  Traditional forum standard  
  • SMF           Simple Machines Forum
  • IPB/Invision  Invision Power Board

MODERN FORUMS
  • XenForo       Modern BBCode with extensions
  • Discourse     Markdown-based discussions

CHAT PLATFORMS:
  Discord       - **bold** *italic* ~~strike~~
  Slack         - *bold* _italic_ ~strike~


PLATFORM DIFFERENCES:
- Legacy forums use [tag] BBCode syntax
- Discord uses **markdown** with double asterisks
- Slack uses *single asterisks* for bold
- Some tags only work on specific platforms

TIP: Select the correct platform before generating
     code to ensure compatibility!
",

        ["3. Text Formatting"] = @"

                      TEXT FORMATTING

BASIC STYLES:
  Bold          [b]text[/b]           **text**
  Italic        [i]text[/i]           *text*
  Underline     [u]text[/u]           __text__
  Strikethrough [s]text[/s]           ~~text~~

COLOR:
  [color=red]Red text[/color]
  [color=#FF6600]Orange text[/color]

SIZE (1-7 scale for most forums):
  [size=5]Large text[/size]
  [size=2]Small text[/size]

FONT:
  [font=Arial]Different font[/font]

COMBINING STYLES:
  [b][i][color=blue]Bold italic blue[/color][/i][/b]

NOTE: Apply styles from inside out:
      First content, then style tags wrap around it.
",

        ["4. Links & Media"] = @"

                      LINKS & MEDIA

URLs:
  [url]https://example.com[/url]
  [url=https://example.com]Click Here[/url]

  Discord/Slack: [text](url) or auto-links

IMAGES:
  [img]https://site.com/image.jpg[/img]

  Markdown: ![alt](url)

YOUTUBE:
  [youtube]VIDEO_ID[/youtube]

  Or paste full URL - the tool extracts the ID

EMAIL:
  [email]user@example.com[/email]
  [email=user@example.com]Contact[/email]

MENTIONS:
  @username

  Platform-specific formats handled automatically

BATCH OPERATIONS:
  Enter multiple URLs (one per line)
  Use 'URLs' or 'Imgs' buttons for batch processing
",

        ["5. Structure Elements"] = @"

                    STRUCTURE ELEMENTS

QUOTES:
  [quote]Quoted text[/quote]
  [quote=Author]Their words[/quote]

CODE BLOCKS:
  [code]your code here[/code]
  [code=python]print('hello')[/code]

SPOILERS:
  [spoiler]Hidden content[/spoiler]
  [spoiler=Click to reveal]Surprise![/spoiler]

LISTS:
  Bullet:    [list][*]Item[/list]
  Numbered:  [list=1][*]First[/list]
  Lettered:  [list=a][*]Alpha[/list]

TABLES:
  Use the Table button to build tables visually.
  Enter data in the grid, toggle header row.

ALIGNMENT:
  [center]Centered text[/center]
  [left] [right] [justify]

HEADERS:
  H1-H6 support varies by platform
  Uses [size] + [b] for BBCode
  Uses # ## ### for Markdown
",

        ["6. Theme Guide"] = @"

                        THEME GUIDE

This application includes 12 visual themes:

DARK THEMES:
  Dark Mode      - Default, professional dark theme
  Matrix         - Green terminal aesthetic
  Neon           - Vibrant magenta and cyan
  Cyberpunk      - Red and gold futuristic
  Ocean          - Cool blue tones
  Synthwave      - Retro 80s pink/purple

LIGHT THEMES:
  Light Mode     - Clean, minimal light theme
  Solarized Light- Warm, easy on the eyes

DEVELOPER THEMES:
  Solarized Dark - Popular code editor theme
  Monokai        - Sublime Text inspired
  Nord           - Arctic color palette

ACCESSIBILITY:
  High Contrast  - Maximum readability


Switch themes using the dropdown at the top.
Your preference is saved between sessions.
",

        ["7. Keyboard Shortcuts"] = @"

                    KEYBOARD SHORTCUTS

TEXT FORMATTING:
  Ctrl+B       Bold
  Ctrl+I       Italic
  Ctrl+U       Underline

LINKS:
  Ctrl+K       Insert URL dialog

GENERAL:
  F1           Open Help
  Escape       Close dialogs

DIALOG NAVIGATION:
  Tab          Move to next field
  Enter        Confirm / OK
  Escape       Cancel / Close

TEXT EDITING:
  Ctrl+A       Select all
  Ctrl+C       Copy
  Ctrl+V       Paste
  Ctrl+Z       Undo
  Ctrl+Y       Redo


TIP: After generating code with auto-copy enabled,
     just press Ctrl+V to paste anywhere!
",

        ["8. Color Reference"] = @"

                      COLOR REFERENCE

NAMED COLORS (widely supported):
  Basic:    red, green, blue, yellow, orange
  Extended: purple, pink, cyan, magenta
  Neutral:  white, black, gray, silver

SPECIAL COLORS:
  gold, navy, teal, maroon, olive
  lime, aqua, fuchsia, coral, salmon

HEX COLORS (format: #RRGGBB):
  #FF0000 = Red         #00FF00 = Green
  #0000FF = Blue        #FFFF00 = Yellow
  #FF00FF = Magenta     #00FFFF = Cyan
  #FFA500 = Orange      #800080 = Purple
  #008080 = Teal        #FFD700 = Gold

USAGE:
  [color=red]Named color[/color]
  [color=#FF6600]Hex color[/color]

TIP: Use the Color dropdown and select 'Custom...'
     to pick any color using the color picker.
",

        ["9. Troubleshooting"] = @"

                     TROUBLESHOOTING

COMMON ISSUES:

  BBCode not rendering?
    - Check if the forum supports that tag
    - Some forums disable certain tags
    - Try simpler formatting first
    - Verify you selected the correct platform

  Images not showing?
    - URL must end in .jpg/.png/.gif
    - Image must be publicly accessible
    - Some forums require image host approval
    - Check for HTTPS requirements

  Clipboard not working?
    - Another app may be locking clipboard
    - Wait a moment and try again
    - Check Windows clipboard settings

  Settings not saving?
    - Check %AppData%/KZBBCode folder permissions
    - Run as administrator if needed

  Application won't start?
    - Requires Windows 10/11 64-bit
    - Check antivirus isn't blocking
    - Try running as Administrator

NEED MORE HELP?
  Report issues at the project repository
",

        ["10. About"] = @"
╔═══════════════════════════════════════════════════════════════╗
║  _  __ ______   ____  ____   ____          _                  ║
║ | |/ /|___  /  | __ )| __ ) / ___|___   __| | ___             ║
║ | ' /    / /   |  _ \|  _ \| |   / _ \ / _` |/ _ \            ║
║ | . \   / /_   | |_) | |_) | |__| (_) | (_| |  __/            ║
║ |_|\_\ /____|  |____/|____/ \____\___/ \__,_|\___|            ║
║                                                               ║
║                      VERSION 3.0                              ║
╚═══════════════════════════════════════════════════════════════╝

DEVELOPER        KrazyZone LAZYFROG
TECHNOLOGY       C# .NET 9 • Windows Forms • MVVM
LICENSE          MIT License

───────────────────────────────────────────────────────────────
FEATURES
───────────────────────────────────────────────────────────────
  ✓ 10 Platform Support    Forums & Chat
  ✓ 12 Visual Themes       Dark, Light, Custom
  ✓ 20+ Tag Types          Text, Media, Structure
  ✓ Auto-Copy              Instant clipboard
  ✓ Keyboard Shortcuts     Power user friendly

───────────────────────────────────────────────────────────────
Making forum formatting beautiful - one BBCode at a time.

                    LAZYFROG
           Keeping the retro web alive
"
    };

    public static string GetSection(int index)
    {
        var keys = Sections.Keys.ToArray();
        return index >= 0 && index < keys.Length ? Sections[keys[index]] : "Section not found.";
    }

    public static string[] GetSectionTitles() => Sections.Keys.ToArray();
}
