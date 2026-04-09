namespace CorleyEngine;

/// <summary>
/// A custom log entry class for reporting to the console.
/// </summary>
public struct LogEntry {

    /// <summary>
    /// The type of log this entry falls under.
    /// </summary>
    public LogType Type;

    /// <summary>
    /// The content of the log entry.
    /// </summary>
    public string Message;

    /// <summary>
    /// The time that this entry was logged.
    /// </summary>
    public string Timestamp;

}