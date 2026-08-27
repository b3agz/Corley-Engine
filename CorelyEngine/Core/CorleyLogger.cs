using System;

namespace CorleyEngine.Core;

/// <summary>
/// Provides static logging functionality.
/// </summary>
public static class CorleyLog {

    /// <summary>
    /// Logs a message to the console with a specific severity level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="level">The severity level of the log message.</param>
    public static void Log(string message, CorleyLogLevel level) {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        Console.WriteLine($"[{timestamp}] [{level}] {message}");
    }

    /// <summary>
    /// Logs a debug message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogDebug(string message) {
        Log(message, CorleyLogLevel.Debug);
    }

    /// <summary>
    /// Logs an info message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogInfo(string message) {
        Log(message, CorleyLogLevel.Info);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogWarning(string message) {
        Log(message, CorleyLogLevel.Warning);
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogError(string message) {
        Log(message, CorleyLogLevel.Error);
    }
}
