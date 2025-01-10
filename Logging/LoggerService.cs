using System;
using System.IO;

namespace HospitalManagementApp.Logging
{
    public class LoggerService : ILoggerService
    {
        private static string _logFile = "log.txt"; // Default log file location

        // Optional: You can change the log file location if needed.
        public static void Initialize(string filePath)
        {
            _logFile = filePath;
            File.WriteAllText(_logFile, "Log Initialized\n");
        }

        public void LogInformation(string message)
        {
            // Log to console
            Console.WriteLine($"INFO: {message}");

            // Optionally log to a file
            File.AppendAllText(_logFile, $"{DateTime.Now}: INFO: {message}\n");
        }

        public void LogError(string message)
        {
            // Log to console
            Console.WriteLine($"ERROR: {message}");

            // Optionally log to a file
            File.AppendAllText(_logFile, $"{DateTime.Now}: ERROR: {message}\n");
        }
    }
}
