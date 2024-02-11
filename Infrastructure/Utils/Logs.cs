using System.Diagnostics;
using System.Text;

namespace Infrastructure.Utils;

public class Logs : ILogs
{
    private readonly string? _filePath;
    private readonly bool _appendToFile;

    public Logs(string? filePath, bool appendToFile)
    {
        _filePath = filePath;
        _appendToFile = appendToFile;
        InitializeLogFile();
    }

    public void InitializeLogFile()
    {
        try
        {
            if (_filePath != null && !File.Exists(_filePath))
            {
                using (var fileStream = File.Create(_filePath))
                {
                    var initialContent = "Log File Created at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var initialContentBytes = Encoding.UTF8.GetBytes(initialContent);
                    fileStream.Write(initialContentBytes, 0, initialContentBytes.Length);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"LOG FILE INITIALIZATION ERROR! {DateTime.Now} :: {ex.Message}");
        }
    }

    public string? FilePath => _filePath;
    public bool AppendToFile => _appendToFile;

    public async Task LogToFileAsync(string message, string methodName = "")
    {
        try
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}, method:{methodName} :: {message}";

            Debug.WriteLine(logMessage);

            if (_filePath != null)
            {
                using (var sw = new StreamWriter(_filePath, _appendToFile, Encoding.UTF8))
                {
                    await sw.WriteLineAsync(logMessage);
                }
            }
            else
            {
                Debug.WriteLine("File path is null. Unable to log to file.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(message: $"LOG ERROR! {DateTime.Now:yyyy-MM-dd HH:mm:ss}, method:{nameof(LogToFileAsync)} :: {ex}");
        }
    }

    public async Task LogWarningAsync(string warningMessage, string methodName = "")
    {
        try
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}, method:{methodName} :: WARNING: {warningMessage}";

            Debug.WriteLine(logMessage);

            if (_filePath != null)
            {
                using (var sw = new StreamWriter(_filePath, _appendToFile, Encoding.UTF8))
                {
                    await sw.WriteLineAsync(logMessage);
                }
            }
            else
            {
                Debug.WriteLine("File path is null. Unable to log to file.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(message: $"LOG WARNING ERROR! {DateTime.Now:yyyy-MM-dd HH:mm:ss}, method:{nameof(LogWarningAsync)} :: {ex}");
        }
    }
}
