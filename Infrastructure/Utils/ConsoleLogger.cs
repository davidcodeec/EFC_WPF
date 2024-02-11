namespace Infrastructure.Utils;

public class ConsoleLogger : ILogs
{
    public void InitializeLogFile()
    {
        // Initialization logic (if any)
    }

    public string FilePath => string.Empty; // Replace with the actual file path if applicable

    public bool AppendToFile => false; // Replace with the actual behavior if applicable

    public async Task LogToFileAsync(string message, string methodName = "")
    {
        Console.WriteLine($"[{DateTime.Now}] method:{methodName} :: {message}");
        await Task.CompletedTask;
    }

    public async Task LogWarningAsync(string warningMessage, string methodName = "")
    {
        Console.WriteLine($"[{DateTime.Now}] method:{methodName} :: WARNING: {warningMessage}");
        await Task.CompletedTask;
    }
}

