namespace KZBBCode.Views.Dialogs;

/// <summary>
/// Forum-style emoji/smiley picker dialog inspired by classic forum shoutbox interfaces.
/// Displays a grid of clickable emojis that insert BBCode-style emoji codes.
/// </summary>
/// <remarks>
/// <para>Emojis are organized into categories:</para>
/// <list type="bullet">
///   <item><description>Classic smileys - :) :D ;) etc.</description></item>
///   <item><description>Forum classics - :thumbsup: :fire: :heart: etc.</description></item>
///   <item><description>Expressions - :lol: :evil: :shock: etc.</description></item>
///   <item><description>Actions - :coffee: :party: :trophy: etc.</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// using var picker = new EmojiPicker();
/// picker.ApplyTheme(bgColor, fgColor, btnColor);
/// if (picker.ShowDialog() == DialogResult.OK)
/// {
///     string emojiCode = picker.SelectedEmoji; // e.g., ":thumbsup:"
/// }
/// </code>
/// </example>
public class EmojiPicker : Form
{
    #region Properties

    /// <summary>
    /// Gets the selected emoji code (e.g., ":thumbsup:") after dialog closes with OK.
    /// Returns null if dialog was cancelled.
    /// </summary>
    public string? SelectedEmoji { get; private set; }

    #endregion

    #region Emoji Data

    /// <summary>
    /// Static collection of available emojis with their BBCode, display character, and name.
    /// </summary>
    private static readonly (string Code, string Display, string Name)[] Emojis = new[]
    {
        // Classic smileys
        (":)", "😊", "Smile"),
        (":D", "😃", "Big Grin"),
        (";)", "😉", "Wink"),
        (":(", "😢", "Sad"),
        (":P", "😛", "Tongue"),
        (":O", "😮", "Surprised"),
        ("XD", "🤣", "Laughing"),
        (":|", "😐", "Neutral"),
        (":angry:", "😠", "Angry"),
        (":cool:", "😎", "Cool"),
        (":cry:", "😭", "Crying"),
        (":love:", "😍", "Love"),
        
        // Forum classics
        (":thumbsup:", "👍", "Thumbs Up"),
        (":thumbsdown:", "👎", "Thumbs Down"),
        (":clap:", "👏", "Clap"),
        (":wave:", "👋", "Wave"),
        (":think:", "🤔", "Thinking"),
        (":facepalm:", "🤦", "Facepalm"),
        (":shrug:", "🤷", "Shrug"),
        (":fire:", "🔥", "Fire"),
        (":star:", "⭐", "Star"),
        (":heart:", "❤️", "Heart"),
        
        // Expressions
        (":lol:", "😂", "LOL"),
        (":rofl:", "🤣", "ROFL"),
        (":evil:", "😈", "Evil"),
        (":twisted:", "👿", "Twisted"),
        (":shock:", "😱", "Shocked"),
        (":confused:", "😕", "Confused"),
        (":sleep:", "😴", "Sleeping"),
        (":sick:", "🤢", "Sick"),
        (":mad:", "🤬", "Mad"),
        (":nervous:", "😬", "Nervous"),
        
        // Actions
        (":coffee:", "☕", "Coffee"),
        (":beer:", "🍺", "Beer"),
        (":party:", "🎉", "Party"),
        (":music:", "🎵", "Music"),
        (":trophy:", "🏆", "Trophy"),
        (":gift:", "🎁", "Gift"),
        (":idea:", "💡", "Idea"),
        (":question:", "❓", "Question"),
        (":warning:", "⚠️", "Warning"),
        (":check:", "✅", "Check"),
        (":x:", "❌", "X Mark"),
        (":info:", "ℹ️", "Info")
    };

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the EmojiPicker dialog.
    /// </summary>
    public EmojiPicker()
    {
        Text = "Insert Emoji";
        Size = new Size(420, 340);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;

        InitializeComponents();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes all UI components including the emoji grid and buttons.
    /// </summary>
    private void InitializeComponents()
    {
        // Title label
        var titleLabel = new Label
        {
            Text = "Click an emoji to insert its BBCode",
            Dock = DockStyle.Top,
            Height = 25,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 9F, FontStyle.Italic)
        };

        // Emoji grid panel
        var emojiPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(10),
            FlowDirection = FlowDirection.LeftToRight
        };

        foreach (var (code, display, name) in Emojis)
        {
            var btn = new Button
            {
                Text = display,
                Size = new Size(42, 42),
                Font = new Font("Segoe UI Emoji", 16F),
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(2),
                Tag = code,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 1;
            
            var tooltip = new ToolTip();
            tooltip.SetToolTip(btn, $"{name}\n{code}");

            btn.Click += (s, e) =>
            {
                SelectedEmoji = code;
                DialogResult = DialogResult.OK;
                Close();
            };

            btn.MouseEnter += (s, e) =>
            {
                btn.FlatAppearance.BorderColor = Color.FromArgb(100, 149, 237);
                btn.BackColor = Color.FromArgb(60, 60, 60);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
                btn.BackColor = Color.FromArgb(45, 45, 45);
            };

            emojiPanel.Controls.Add(btn);
        }

        // Bottom panel with close button
        var bottomPanel = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 45
        };

        var closeBtn = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Size = new Size(80, 30),
            Location = new Point(160, 8)
        };
        bottomPanel.Controls.Add(closeBtn);

        Controls.Add(emojiPanel);
        Controls.Add(titleLabel);
        Controls.Add(bottomPanel);
        
        CancelButton = closeBtn;
    }

    /// <summary>
    /// Apply theme colors to the picker
    /// </summary>
    public void ApplyTheme(Color background, Color foreground, Color buttonBg)
    {
        BackColor = background;
        ForeColor = foreground;

        foreach (Control ctrl in Controls)
        {
            ctrl.BackColor = background;
            ctrl.ForeColor = foreground;

            if (ctrl is FlowLayoutPanel panel)
            {
                foreach (Control btn in panel.Controls)
                {
                    if (btn is Button b)
                    {
                        b.BackColor = buttonBg;
                        b.ForeColor = foreground;
                        b.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
                    }
                }
            }
        }
    }

    #endregion
}
