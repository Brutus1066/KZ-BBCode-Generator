using KZBBCode.Views;

namespace KZBBCode;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Handle unhandled exceptions
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (s, e) => HandleException(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
                HandleException(ex);
        };

        Application.Run(new MainForm());
    }

    private static void HandleException(Exception ex)
    {
        MessageBox.Show(
            $"An unexpected error occurred:\n\n{ex.Message}\n\nThe application will continue running.",
            "KZ BBCode Generator - Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        );
    }
}
