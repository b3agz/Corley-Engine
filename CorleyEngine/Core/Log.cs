using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CorleyEngine;

/// <summary>
/// Class containing methods for logging messages to the console.
/// </summary>
public static class Log {

    public const int MAX_MESSAGE_HISTORY = 100;

    /// <summary>
    /// A list of past logs for displaying in UI consoles and in-editor.
    /// </summary>
    public static List<string> History { get; private set; } = [];

    // TODO: Add object context like Unity has for highlighting Entities associated with the log.

    /// <summary>
    /// Logs a message to the console.
    /// </summary>
    [Conditional("DEBUG")]
    public static void Info(string message) {
        Write("[DEBUG]", message, ConsoleColor.White);
    }

    /// <summary>
    /// Logs a warning to the console.
    /// </summary>
    [Conditional("DEBUG")]
    public static void Warning(string message) {
        Write("[WARNING]", message, ConsoleColor.Yellow);
    }

    /// <summary>
    /// Logs an error to the console.
    /// </summary>
    [Conditional("DEBUG")]
    public static void Error(string message) {
        Write("[ERROR]", message, ConsoleColor.Red);
    }

    private static void Write(string level, string message, ConsoleColor color) {

        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string fullMessage = $"[{timestamp}] [{level}] {message}";

        History.Add(fullMessage);
        if (History.Count > MAX_MESSAGE_HISTORY) History.RemoveAt(0);

        // Write to the IDE output
        Console.ForegroundColor = color;
        Debug.WriteLine(fullMessage);
        Console.ResetColor();

    }
}
