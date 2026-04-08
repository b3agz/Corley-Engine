using System;
using System.Collections.Generic;

namespace CorleyEngine.Core;

/// <summary>
/// A custom logging class that allows for error reporting in the engine.
/// </summary>
public static class EngineLogger {

    /// <summary>
    /// The master list of all logs.
    /// </summary>
    public static readonly List<LogEntry> Logs = [];

    /// <summary>
    /// A public event that fires any time a new log is added.
    /// </summary>
    public static event Action OnLogAdded;

    public static void Info(string message) => AddLog(LogType.Info, message);
    public static void Warning(string message) => AddLog(LogType.Warning, message);
    public static void Error(string message) => AddLog(LogType.Error, message);

    private static void AddLog(LogType type, string message) {
        Logs.Add(new LogEntry {
            Type = type,
            Message = message,
            Timestamp = DateTime.Now.ToString("HH:mm:ss")
        });

        // Tell anything listening that a new message arrived
        OnLogAdded?.Invoke();
    }

    /// <summary>
    /// Clears the log of its history.
    /// </summary>
    public static void Clear() {
        Logs.Clear();
    }
}