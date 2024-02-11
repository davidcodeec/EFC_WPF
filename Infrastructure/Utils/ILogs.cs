
namespace Infrastructure.Utils
{
    public interface ILogs
    {
        bool AppendToFile { get; }
        string? FilePath { get; }

        void InitializeLogFile();
        Task LogToFileAsync(string message, string methodName = "");
        Task LogWarningAsync(string warningMessage, string methodName = "");
    }
}