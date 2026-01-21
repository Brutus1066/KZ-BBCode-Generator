namespace KZBBCode.Views.Dialogs;

/// <summary>
/// Generic modal dialog for collecting single or multiple text inputs from the user.
/// </summary>
/// <remarks>
/// <para>This dialog supports dynamically adding multiple labeled text fields
/// using the <see cref="AddField"/> method. Fields are stacked vertically
/// and the dialog automatically resizes to accommodate them.</para>
/// 
/// <para>Returns <see cref="DialogResult.OK"/> when the user clicks OK,
/// or <see cref="DialogResult.Cancel"/> when cancelled or closed.</para>
/// </remarks>
/// <example>
/// <code>
/// using var dialog = new InputDialog("Insert URL", "Enter URL:", "https://");
/// dialog.AddField("Display text:", "Click here");
/// if (dialog.ShowDialog() == DialogResult.OK)
/// {
///     var url = dialog.GetValue(0);
///     var text = dialog.GetValue(1);
/// }
/// </code>
/// </example>
public class InputDialog : Form
{
    #region Fields

    private readonly List<TextBox> _textBoxes = new();
    private readonly TableLayoutPanel _layout;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new input dialog with a single text field.
    /// </summary>
    /// <param name="title">Dialog window title.</param>
    /// <param name="label">Label for the first text field.</param>
    /// <param name="defaultValue">Default value for the first text field.</param>
    public InputDialog(string title, string label, string defaultValue = "")
    {
        Text = title;
        Size = new Size(400, 150);
        MinimumSize = new Size(350, 130);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        _layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            Padding = new Padding(10)
        };

        AddField(label, defaultValue);

        // Buttons
        var buttonPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Bottom,
            Height = 40,
            Padding = new Padding(5)
        };

        var cancelBtn = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 75 };
        var okBtn = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 75 };

        buttonPanel.Controls.Add(cancelBtn);
        buttonPanel.Controls.Add(okBtn);

        AcceptButton = okBtn;
        CancelButton = cancelBtn;

        Controls.Add(_layout);
        Controls.Add(buttonPanel);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adds an additional text field to the dialog.
    /// </summary>
    /// <param name="label">Label for the new field.</param>
    /// <param name="defaultValue">Default value for the field.</param>
    public void AddField(string label, string defaultValue)
    {
        var fieldLabel = new Label
        {
            Text = label,
            AutoSize = true,
            Margin = new Padding(0, 5, 0, 2)
        };

        var textBox = new TextBox
        {
            Text = defaultValue,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 0, 0, 10)
        };

        _textBoxes.Add(textBox);
        _layout.RowCount++;
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.Controls.Add(fieldLabel);
        _layout.RowCount++;
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.Controls.Add(textBox);

        // Adjust form height
        Height += 50;
    }

    /// <summary>
    /// Gets the value from a text field by index.
    /// </summary>
    /// <param name="index">Zero-based index of the field.</param>
    /// <returns>The text value, or empty string if index is out of range.</returns>
    public string GetValue(int index = 0)
    {
        return index >= 0 && index < _textBoxes.Count ? _textBoxes[index].Text : string.Empty;
    }

    /// <summary>
    /// Gets all text field values in order.
    /// </summary>
    /// <returns>Enumerable of all field values.</returns>
    public IEnumerable<string> GetAllValues()
    {
        return _textBoxes.Select(t => t.Text);
    }

    #endregion
}
