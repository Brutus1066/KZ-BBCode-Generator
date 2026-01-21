namespace KZBBCode.Views.Dialogs;

/// <summary>
/// Dialog for building tables with customizable dimensions and content.
/// </summary>
/// <remarks>
/// <para>Provides a visual table editor with adjustable row and column counts.
/// Users can edit cell content directly in a DataGridView and optionally
/// designate the first row as a header row.</para>
/// 
/// <para>The dialog preserves existing cell data when resizing the table,
/// copying values to the new grid where positions overlap.</para>
/// </remarks>
/// <example>
/// <code>
/// using var dialog = new TableDialog();
/// if (dialog.ShowDialog() == DialogResult.OK)
/// {
///     var cells = dialog.GetTableData();
///     var hasHeader = dialog.HasHeader;
///     var bbcode = generator.Table(cells, hasHeader);
/// }
/// </code>
/// </example>
public class TableDialog : Form
{
    #region Fields

    private NumericUpDown _rowsInput = null!;
    private NumericUpDown _colsInput = null!;
    private DataGridView _grid = null!;
    private CheckBox _headerCheck = null!;

    #endregion

    #region Properties

    /// <summary>
    /// Gets whether the first row should be treated as a header row.
    /// </summary>
    public bool HasHeader => _headerCheck.Checked;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new table builder dialog.
    /// </summary>
    public TableDialog()
    {
        Text = "Insert Table";
        Size = new Size(500, 400);
        MinimumSize = new Size(400, 350);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.Sizable;
        MaximizeBox = false;
        MinimizeBox = false;

        InitializeComponents();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes all UI components and layout.
    /// </summary>
    private void InitializeComponents()
    {
        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(10)
        };
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

        // Size controls
        var sizePanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight
        };

        var rowsLabel = new Label { Text = "Rows:", AutoSize = true, Margin = new Padding(0, 6, 5, 0) };
        _rowsInput = new NumericUpDown { Minimum = 1, Maximum = 50, Value = 3, Width = 60 };
        _rowsInput.ValueChanged += OnSizeChanged;

        var colsLabel = new Label { Text = "Columns:", AutoSize = true, Margin = new Padding(15, 6, 5, 0) };
        _colsInput = new NumericUpDown { Minimum = 1, Maximum = 20, Value = 3, Width = 60 };
        _colsInput.ValueChanged += OnSizeChanged;

        _headerCheck = new CheckBox
        {
            Text = "First row is header",
            Checked = true,
            AutoSize = true,
            Margin = new Padding(20, 5, 0, 0)
        };

        sizePanel.Controls.AddRange(new Control[] { rowsLabel, _rowsInput, colsLabel, _colsInput, _headerCheck });
        mainLayout.Controls.Add(sizePanel, 0, 0);

        // Grid
        _grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            SelectionMode = DataGridViewSelectionMode.CellSelect
        };
        mainLayout.Controls.Add(_grid, 0, 1);

        // Buttons
        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 5, 0, 0)
        };

        var cancelBtn = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 75 };
        var okBtn = new Button { Text = "Insert", DialogResult = DialogResult.OK, Width = 75 };

        buttonPanel.Controls.Add(cancelBtn);
        buttonPanel.Controls.Add(okBtn);

        AcceptButton = okBtn;
        CancelButton = cancelBtn;

        mainLayout.Controls.Add(buttonPanel, 0, 2);

        Controls.Add(mainLayout);

        // Initialize grid
        UpdateGrid();
    }

    #endregion

    #region Event Handlers

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        UpdateGrid();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the grid to match the current row/column settings.
    /// </summary>
    private void UpdateGrid()
    {
        var rows = (int)_rowsInput.Value;
        var cols = (int)_colsInput.Value;

        // Store existing data
        var existingData = new string[_grid.RowCount, _grid.ColumnCount];
        for (int r = 0; r < _grid.RowCount; r++)
        {
            for (int c = 0; c < _grid.ColumnCount; c++)
            {
                existingData[r, c] = _grid.Rows[r].Cells[c].Value?.ToString() ?? "";
            }
        }

        // Rebuild grid
        _grid.Columns.Clear();
        _grid.Rows.Clear();

        for (int c = 0; c < cols; c++)
        {
            _grid.Columns.Add($"col{c}", $"Column {c + 1}");
        }

        for (int r = 0; r < rows; r++)
        {
            var rowIndex = _grid.Rows.Add();
            for (int c = 0; c < cols; c++)
            {
                // Restore existing data if available
                if (r < existingData.GetLength(0) && c < existingData.GetLength(1))
                {
                    _grid.Rows[rowIndex].Cells[c].Value = existingData[r, c];
                }
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the table data as a 2D string array.
    /// </summary>
    /// <returns>2D array with [rows, columns] containing cell values.</returns>
    public string[,] GetTableData()
    {
        var rows = _grid.RowCount;
        var cols = _grid.ColumnCount;
        var data = new string[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                data[r, c] = _grid.Rows[r].Cells[c].Value?.ToString() ?? "";
            }
        }

        return data;
    }

    #endregion
}
