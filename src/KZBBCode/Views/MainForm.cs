using KZBBCode.Models;
using KZBBCode.Services;
using KZBBCode.ViewModels;
using KZBBCode.Views.Dialogs;

namespace KZBBCode.Views;

/// <summary>
/// Main application window for KZ BBCode Generator.
/// Implements a responsive MVVM-based layout with theming support.
/// </summary>
/// <remarks>
/// <para>The form consists of four main sections:</para>
/// <list type="bullet">
///   <item><description>Header - Platform and theme selection</description></item>
///   <item><description>Toolbar - Quick access formatting buttons</description></item>
///   <item><description>Content - Split panel with input/output text areas</description></item>
///   <item><description>Status bar - Current state information</description></item>
/// </list>
/// </remarks>
public class MainForm : Form
{
    #region Fields

    private readonly MainViewModel _viewModel;
    private Theme _currentTheme;

    // UI Controls
    private ComboBox _platformCombo = null!;
    private ComboBox _themeCombo = null!;
    private RichTextBox _inputTextBox = null!;
    private RichTextBox _outputTextBox = null!;
    private StatusStrip _statusBar = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private ToolStrip _toolbar = null!;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the MainForm class.
    /// </summary>
    public MainForm()
    {
        _viewModel = new MainViewModel();
        _currentTheme = ThemeService.GetByName(_viewModel.SelectedTheme);
        InitializeComponents();
        SetupBindings();
        ApplyTheme();
        RestoreWindowState();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes all UI components and sets up the form layout.
    /// </summary>
    private void InitializeComponents()
    {
        Text = "KZ BBCode Generator v3.0";
        Size = new Size(1000, 700);
        MinimumSize = new Size(800, 600);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9F);
        KeyPreview = true;

        // Try to load icon from resources
        try
        {
            var iconPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Desktop.BBCode.KZ.icon.ico");
            if (File.Exists(iconPath))
                Icon = new Icon(iconPath);
        }
        catch { /* Icon loading is optional */ }

        // Main layout using TableLayoutPanel for responsive design
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(5)
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));  // Header
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));  // Toolbar
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Content
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));  // Status

        // Build UI sections
        var headerPanel = CreateHeaderPanel();
        mainLayout.Controls.Add(headerPanel, 0, 0);

        _toolbar = CreateToolbar();
        mainLayout.Controls.Add(_toolbar, 0, 1);

        var contentPanel = CreateContentPanel();
        mainLayout.Controls.Add(contentPanel, 0, 2);

        _statusBar = CreateStatusBar();
        mainLayout.Controls.Add(_statusBar, 0, 3);

        Controls.Add(mainLayout);

        // Wire up events
        FormClosing += OnFormClosing;
        KeyDown += OnKeyDown;
    }

    #endregion

    #region UI Creation

    /// <summary>
    /// Creates the header panel with platform/theme selectors and controls.
    /// </summary>
    /// <returns>Configured header panel</returns>
    private Panel CreateHeaderPanel()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(5, 0, 5, 0)
        };

        // App title/logo
        var titleLabel = new Label 
        { 
            Text = "KZ", 
            AutoSize = true, 
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            Margin = new Padding(0, 3, 10, 0),
            ForeColor = Color.FromArgb(100, 149, 237)
        };

        // Platform selector
        var platformLabel = new Label { Text = "Platform:", AutoSize = true, Margin = new Padding(0, 7, 5, 0) };
        _platformCombo = new ComboBox
        {
            Width = 130,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(0, 3, 15, 0),
            FlatStyle = FlatStyle.Flat
        };
        _platformCombo.Items.AddRange(_viewModel.PlatformNames.ToArray());
        _platformCombo.SelectedItem = PlatformInfo.GetByType(_viewModel.SelectedPlatform).Name;
        _platformCombo.SelectedIndexChanged += (s, e) =>
        {
            if (_platformCombo.SelectedItem is string name)
            {
                _viewModel.SetPlatformByName(name);
                UpdateStatus("Platform changed");
            }
        };

        // Theme selector
        var themeLabel = new Label { Text = "Theme:", AutoSize = true, Margin = new Padding(0, 7, 5, 0) };
        _themeCombo = new ComboBox
        {
            Width = 120,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Margin = new Padding(0, 3, 15, 0),
            FlatStyle = FlatStyle.Flat
        };
        _themeCombo.Items.AddRange(_viewModel.ThemeNames.ToArray());
        _themeCombo.SelectedItem = _viewModel.SelectedTheme;
        _themeCombo.SelectedIndexChanged += (s, e) =>
        {
            if (_themeCombo.SelectedItem is string name)
            {
                _viewModel.SelectedTheme = name;
                _currentTheme = ThemeService.GetByName(name);
                ApplyTheme();
            }
        };

        // Spacer
        var spacer = new Panel { Width = 120, Height = 1 };

        // Auto-copy toggle with icon
        var autoCopyCheck = new CheckBox
        {
            Text = "📋 Auto-copy",
            AutoSize = true,
            Checked = _viewModel.AutoCopy,
            Margin = new Padding(10, 5, 10, 0),
            Cursor = Cursors.Hand
        };
        autoCopyCheck.CheckedChanged += (s, e) => _viewModel.AutoCopy = autoCopyCheck.Checked;

        // Settings button with dropdown menu
        var settingsBtn = new Button 
        { 
            Text = "⚙️", 
            Width = 32, 
            Height = 28, 
            Margin = new Padding(5, 2, 0, 0),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        settingsBtn.FlatAppearance.BorderSize = 0;
        
        var settingsMenu = new ContextMenuStrip();
        settingsMenu.Items.Add("Create Desktop Shortcut", null, (s, e) => CreateDesktopShortcut());
        settingsMenu.Items.Add(new ToolStripSeparator());
        settingsMenu.Items.Add("Open Settings Folder", null, (s, e) => 
        {
            var folder = SettingsService.GetSettingsFolder();
            if (Directory.Exists(folder))
                System.Diagnostics.Process.Start("explorer.exe", folder);
        });
        settingsMenu.Items.Add("Reset Settings", null, (s, e) =>
        {
            var result = MessageBox.Show(
                "Are you sure you want to reset all settings to defaults?",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Yes)
            {
                var settingsFile = Path.Combine(SettingsService.GetSettingsFolder(), "settings.json");
                if (File.Exists(settingsFile))
                {
                    File.Delete(settingsFile);
                    MessageBox.Show("Settings reset. Please restart the application.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        });
        settingsBtn.Click += (s, e) => settingsMenu.Show(settingsBtn, new Point(0, settingsBtn.Height));

        // Help button
        var helpBtn = new Button 
        { 
            Text = "❓", 
            Width = 32, 
            Height = 28, 
            Margin = new Padding(5, 2, 0, 0),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        helpBtn.FlatAppearance.BorderSize = 0;
        helpBtn.Click += (s, e) => ShowHelp();

        panel.Controls.AddRange(new Control[] { titleLabel, platformLabel, _platformCombo, themeLabel, _themeCombo, spacer, autoCopyCheck, settingsBtn, helpBtn });
        return panel;
    }

    private ToolStrip CreateToolbar()
    {
        var toolbar = new ToolStrip
        {
            Dock = DockStyle.Fill,
            GripStyle = ToolStripGripStyle.Hidden,
            RenderMode = ToolStripRenderMode.Professional,
            Padding = new Padding(2, 0, 2, 0)
        };

        // Format dropdown with all text options
        var formatBtn = new ToolStripDropDownButton("Format ▾") { ToolTipText = "Text formatting" };
        formatBtn.DropDownItems.Add("Bold", null, (s, e) => _viewModel.ApplyBoldCommand.Execute(null));
        formatBtn.DropDownItems.Add("Italic", null, (s, e) => _viewModel.ApplyItalicCommand.Execute(null));
        formatBtn.DropDownItems.Add("Underline", null, (s, e) => _viewModel.ApplyUnderlineCommand.Execute(null));
        formatBtn.DropDownItems.Add("Strikethrough", null, (s, e) => _viewModel.ApplyStrikethroughCommand.Execute(null));
        formatBtn.DropDownItems.Add(new ToolStripSeparator());
        var colorSub = new ToolStripMenuItem("Color");
        colorSub.DropDownItems.AddRange(CreateColorMenuItems());
        formatBtn.DropDownItems.Add(colorSub);
        var sizeSub = new ToolStripMenuItem("Size");
        for (int i = 1; i <= 7; i++)
        {
            var size = i.ToString();
            sizeSub.DropDownItems.Add($"Size {size}", null, (s, e) => _viewModel.ApplySizeCommand.Execute(size));
        }
        formatBtn.DropDownItems.Add(sizeSub);
        toolbar.Items.Add(formatBtn);

        // Quick format buttons
        var bBtn = new ToolStripButton("B") { ToolTipText = "Bold (Ctrl+B)", Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        bBtn.Click += (s, e) => _viewModel.ApplyBoldCommand.Execute(null);
        toolbar.Items.Add(bBtn);
        var iBtn = new ToolStripButton("I") { ToolTipText = "Italic (Ctrl+I)", Font = new Font("Segoe UI", 9F, FontStyle.Italic) };
        iBtn.Click += (s, e) => _viewModel.ApplyItalicCommand.Execute(null);
        toolbar.Items.Add(iBtn);
        var uBtn = new ToolStripButton("U") { ToolTipText = "Underline (Ctrl+U)" };
        uBtn.Click += (s, e) => _viewModel.ApplyUnderlineCommand.Execute(null);
        toolbar.Items.Add(uBtn);
        toolbar.Items.Add(new ToolStripSeparator());

        // Media dropdown
        var mediaBtn = new ToolStripDropDownButton("Media ▾") { ToolTipText = "Links and media" };
        mediaBtn.DropDownItems.Add("URL Link", null, (s, e) => ShowUrlDialog());
        mediaBtn.DropDownItems.Add("Image", null, (s, e) => ShowImageDialog());
        mediaBtn.DropDownItems.Add("YouTube", null, (s, e) => _viewModel.ApplyYouTubeCommand.Execute(null));
        mediaBtn.DropDownItems.Add(new ToolStripSeparator());
        mediaBtn.DropDownItems.Add("Batch URLs", null, (s, e) => _viewModel.BatchUrlsCommand.Execute(null));
        mediaBtn.DropDownItems.Add("Batch Images", null, (s, e) => _viewModel.BatchImagesCommand.Execute(null));
        toolbar.Items.Add(mediaBtn);
        toolbar.Items.Add(new ToolStripSeparator());

        // Insert dropdown
        var insertBtn = new ToolStripDropDownButton("Insert ▾") { ToolTipText = "Structure elements" };
        insertBtn.DropDownItems.Add("Quote", null, (s, e) => ShowQuoteDialog());
        insertBtn.DropDownItems.Add("Code Block", null, (s, e) => ShowCodeDialog());
        insertBtn.DropDownItems.Add("Spoiler", null, (s, e) => ShowSpoilerDialog());
        insertBtn.DropDownItems.Add(new ToolStripSeparator());
        insertBtn.DropDownItems.Add("Bullet List", null, (s, e) => _viewModel.ApplyListCommand.Execute(ListType.Bullet));
        insertBtn.DropDownItems.Add("Numbered List", null, (s, e) => _viewModel.ApplyListCommand.Execute(ListType.Numbered));
        insertBtn.DropDownItems.Add("Table", null, (s, e) => ShowTableDialog());
        insertBtn.DropDownItems.Add(new ToolStripSeparator());
        insertBtn.DropDownItems.Add("Align Left", null, (s, e) => _viewModel.ApplyAlignCommand.Execute(TextAlignment.Left));
        insertBtn.DropDownItems.Add("Align Center", null, (s, e) => _viewModel.ApplyAlignCommand.Execute(TextAlignment.Center));
        insertBtn.DropDownItems.Add("Align Right", null, (s, e) => _viewModel.ApplyAlignCommand.Execute(TextAlignment.Right));
        toolbar.Items.Add(insertBtn);

        // Quick structure buttons
        toolbar.Items.Add(CreateToolButton("Quote", "Quote", () => ShowQuoteDialog()));
        toolbar.Items.Add(CreateToolButton("Code", "Code Block", () => ShowCodeDialog()));
        toolbar.Items.Add(CreateToolButton("Table", "Insert Table", () => ShowTableDialog()));
        toolbar.Items.Add(new ToolStripSeparator());

        // Emoji picker button
        var emojiBtn = new ToolStripButton("😊") { ToolTipText = "Insert Emoji", Font = new Font("Segoe UI Emoji", 12F) };
        emojiBtn.Click += (s, e) => ShowEmojiPicker();
        toolbar.Items.Add(emojiBtn);
        toolbar.Items.Add(new ToolStripSeparator());

        toolbar.Items.Add(CreateToolButton("Clear", "Clear All", () => _viewModel.ClearInputCommand.Execute(null)));

        return toolbar;
    }

    private ToolStripItem[] CreateColorMenuItems()
    {
        var colors = new[]
        {
            ("Red", "red"), ("Green", "green"), ("Blue", "blue"),
            ("Yellow", "yellow"), ("Orange", "orange"), ("Purple", "purple"),
            ("Cyan", "cyan"), ("Magenta", "magenta"), ("White", "white"),
            ("Black", "black"), ("Gray", "gray"), ("Custom...", "")
        };

        return colors.Select(c =>
        {
            var item = new ToolStripMenuItem(c.Item1);
            item.Click += (s, e) =>
            {
                if (c.Item2 == "")
                {
                    using var colorDlg = new ColorDialog();
                    if (colorDlg.ShowDialog() == DialogResult.OK)
                    {
                        var hex = $"#{colorDlg.Color.R:X2}{colorDlg.Color.G:X2}{colorDlg.Color.B:X2}";
                        _viewModel.ApplyColorCommand.Execute(hex);
                    }
                }
                else
                {
                    _viewModel.ApplyColorCommand.Execute(c.Item2);
                }
            };
            return (ToolStripItem)item;
        }).ToArray();
    }

    private static ToolStripButton CreateToolButton(string text, string tooltip, Action onClick)
    {
        var btn = new ToolStripButton(text) { ToolTipText = tooltip };
        btn.Click += (s, e) => onClick();
        return btn;
    }

    private SplitContainer CreateContentPanel()
    {
        var split = new SplitContainer
        {
            Dock = DockStyle.Fill,
            Orientation = Orientation.Vertical,
            SplitterDistance = 450,
            SplitterWidth = 6,
            BorderStyle = BorderStyle.None
        };

        // Input panel with styled header
        var inputPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        var inputHeader = new Panel { Dock = DockStyle.Top, Height = 28 };
        var inputLabel = new Label 
        { 
            Text = "📝 INPUT TEXT", 
            Dock = DockStyle.Fill, 
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        var charCount = new Label
        {
            Text = "0 chars",
            Dock = DockStyle.Right,
            Width = 80,
            TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 8F)
        };
        inputHeader.Controls.Add(inputLabel);
        inputHeader.Controls.Add(charCount);

        _inputTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 10F),
            AcceptsTab = true,
            WordWrap = true,
            BorderStyle = BorderStyle.None
        };
        _inputTextBox.TextChanged += (s, e) => 
        {
            _viewModel.InputText = _inputTextBox.Text;
            charCount.Text = $"{_inputTextBox.Text.Length} chars";
        };
        inputPanel.Controls.Add(_inputTextBox);
        inputPanel.Controls.Add(inputHeader);
        split.Panel1.Controls.Add(inputPanel);

        // Output panel with styled header
        var outputPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };
        var outputHeader = new Panel { Dock = DockStyle.Top, Height = 28 };
        var outputLabel = new Label 
        { 
            Text = "📋 GENERATED CODE", 
            Dock = DockStyle.Fill, 
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Copy button in header
        var copyBtn = new Button
        {
            Text = "📋 Copy",
            Size = new Size(70, 24),
            Dock = DockStyle.Right,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 8F)
        };
        copyBtn.FlatAppearance.BorderSize = 1;
        copyBtn.Click += (s, e) => 
        {
            _viewModel.CopyOutputCommand.Execute(null);
            copyBtn.Text = "✓ Copied!";
            var timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (ts, te) => { copyBtn.Text = "📋 Copy"; timer.Stop(); timer.Dispose(); };
            timer.Start();
        };
        outputHeader.Controls.Add(outputLabel);
        outputHeader.Controls.Add(copyBtn);

        _outputTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Consolas", 10F),
            ReadOnly = true,
            WordWrap = true,
            BorderStyle = BorderStyle.None
        };

        outputPanel.Controls.Add(_outputTextBox);
        outputPanel.Controls.Add(outputHeader);

        split.Panel2.Controls.Add(outputPanel);

        return split;
    }

    private StatusStrip CreateStatusBar()
    {
        var status = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel
        {
            Text = "Ready | Platform: phpBB | Theme: Dark Mode",
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft
        };
        status.Items.Add(_statusLabel);
        return status;
    }

    private void SetupBindings()
    {
        // PropertyChanged bindings
        _viewModel.PropertyChanged += (s, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.OutputText):
                    _outputTextBox.Text = _viewModel.OutputText;
                    break;
                case nameof(MainViewModel.StatusText):
                    _statusLabel.Text = _viewModel.StatusText;
                    break;
            }
        };
    }

    private void ApplyTheme()
    {
        ThemeService.ApplyTheme(this, _currentTheme);
    }

    private void RestoreWindowState()
    {
        SettingsService.RestoreWindowPosition(_viewModel.Settings, this);
    }

    // Dialog methods
    private void ShowUrlDialog()
    {
        using var dlg = new InputDialog("Insert URL", "URL:", _inputTextBox.Text);
        dlg.AddField("Display Text (optional):", "");
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.InputText = dlg.GetValue(0);
            _viewModel.ApplyUrlCommand.Execute(dlg.GetValue(1));
        }
    }

    private void ShowImageDialog()
    {
        using var dlg = new InputDialog("Insert Image", "Image URL:", _inputTextBox.Text);
        dlg.AddField("Caption (optional):", "");
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.InputText = dlg.GetValue(0);
            _viewModel.ApplyImageCommand.Execute(dlg.GetValue(1));
        }
    }

    private void ShowQuoteDialog()
    {
        using var dlg = new InputDialog("Quote", "Author (optional):", "");
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.ApplyQuoteCommand.Execute(dlg.GetValue(0));
        }
    }

    private void ShowCodeDialog()
    {
        using var dlg = new InputDialog("Code Block", "Language (optional):", "");
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.ApplyCodeCommand.Execute(dlg.GetValue(0));
        }
    }

    private void ShowSpoilerDialog()
    {
        using var dlg = new InputDialog("Spoiler", "Title (optional):", "");
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.ApplySpoilerCommand.Execute(dlg.GetValue(0));
        }
    }

    private void ShowTableDialog()
    {
        using var dlg = new TableDialog();
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            _viewModel.ApplyTable(dlg.GetTableData(), dlg.HasHeader);
        }
    }

    private void ShowHelp()
    {
        using var dlg = new HelpDialog();
        ThemeService.ApplyTheme(dlg, _currentTheme);
        dlg.ShowDialog();
    }

    private void ShowEmojiPicker()
    {
        using var dlg = new EmojiPicker();
        dlg.ApplyTheme(_currentTheme.Background, _currentTheme.Foreground, _currentTheme.ButtonBackground);
        if (dlg.ShowDialog() == DialogResult.OK && dlg.SelectedEmoji != null)
        {
            // Insert emoji code at cursor or append
            var emoji = dlg.SelectedEmoji;
            _viewModel.OutputText = emoji;
            UpdateStatus($"Emoji: {emoji}");
        }
    }

    private void UpdateStatus(string message)
    {
        _statusLabel.Text = $"{message} | Platform: {PlatformInfo.GetByType(_viewModel.SelectedPlatform).Name} | Theme: {_viewModel.SelectedTheme}";
    }

    // Event handlers
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control)
        {
            switch (e.KeyCode)
            {
                case Keys.B:
                    _viewModel.ApplyBoldCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Keys.I:
                    _viewModel.ApplyItalicCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Keys.U:
                    _viewModel.ApplyUnderlineCommand.Execute(null);
                    e.Handled = true;
                    break;
                case Keys.K:
                    ShowUrlDialog();
                    e.Handled = true;
                    break;
            }
        }
        else if (e.KeyCode == Keys.F1)
        {
            ShowHelp();
            e.Handled = true;
        }
    }

    private void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        SettingsService.SaveWindowPosition(_viewModel.Settings, this);
        _viewModel.SaveSettings();

        // Ask about desktop shortcut on first exit
        if (!_viewModel.Settings.AskedAboutShortcut)
        {
            _viewModel.Settings.AskedAboutShortcut = true;
            var result = MessageBox.Show(
                "Would you like to create a desktop shortcut?",
                "KZ BBCode Generator",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                CreateDesktopShortcut();
            }
            _viewModel.SaveSettings();
        }
    }

    /// <summary>
    /// Creates a desktop shortcut for the application.
    /// Uses Windows Script Host COM interop to create a .lnk file.
    /// The icon is embedded in the exe, so it persists after reboot.
    /// </summary>
    private void CreateDesktopShortcut()
    {
        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var shortcutPath = Path.Combine(desktopPath, "KZ BBCode Generator.lnk");
            var exePath = Application.ExecutablePath;

            // Use WScript.Shell COM object to create shortcut
            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType == null) return;

            dynamic shell = Activator.CreateInstance(shellType)!;
            var shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
            shortcut.Description = "KZ BBCode Generator v3.0 - Forum formatting made easy";

            // Icon is embedded in the exe via ApplicationIcon in .csproj
            // Point to the exe itself as the icon source (index 0)
            shortcut.IconLocation = $"{exePath},0";

            shortcut.Save();
            
            MessageBox.Show(
                "Desktop shortcut created successfully!",
                "KZ BBCode Generator",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Could not create shortcut: {ex.Message}",
                "KZ BBCode Generator",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

    #endregion
}
