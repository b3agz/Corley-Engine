using System;
using System.IO;
using System.Text.Json;

namespace CorleyEngine.IO;

/// <summary>
/// A static class that provides functions for saving and loading between JSON data and generic types.
/// </summary>
public static class DataSerializer {

    // Centralised JSON options so all JSON files the engine stores are formatted the same.
    private static readonly JsonSerializerOptions _options = new() {
        WriteIndented = true
    };

    /// <summary>
    /// Serializes any object to a formatted JSON file.
    /// </summary>
    /// <typeparam name="T">The data type being serialized.</typeparam>
    /// <param name="data">The object holding the data.</param>
    /// <param name="absolutePath">The absolute path to the file where the data is being saved.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Save<T>(T data, string absolutePath) {

        if (data == null)
            throw new ArgumentNullException(nameof(data));

        // Ensure the target directory actually exists before writing
        string directory = Path.GetDirectoryName(absolutePath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        string jsonContent = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(absolutePath, jsonContent);
    }

    /// <summary>
    /// Deserializes a JSON file into a C# object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The data type being created.</typeparam>
    /// <param name="absolutePath">The absolute path to the file containing JSON data.</param>
    /// <returns>Returns a default object if an issue arises.</returns>
    /// <exception cref="FileNotFoundException">Returns FileNotFoundException if the file does not exist.</exception>
    /// <exception cref="Exception">Returns an Exception if the process fails.</exception>
    public static T Load<T>(string absolutePath) where T : new() {

        if (!File.Exists(absolutePath)) {
            throw new FileNotFoundException($"[DataSerializer] Cannot find file at: {absolutePath}");
        }

        try {

            string jsonContent = File.ReadAllText(absolutePath);

            // In case we find a file but it is blank, return a default object rather than crashing.
            if (string.IsNullOrWhiteSpace(jsonContent)) {
                return new T();
            }

            return JsonSerializer.Deserialize<T>(jsonContent, _options);

        } catch (JsonException ex) {

            throw new Exception($"[DataSerializer] The file at '{absolutePath}' is corrupted or invalid JSON. Error: {ex.Message}");

        }
    }
}