using System;
using System.IO;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using CorleyEngine.IO;
using System.Text.Json.Serialization;

namespace CorleyEngine.Editor;

/// <summary>
/// A class for storing the editor's preferences, designed to be easily serialisable for storage.
/// </summary>
public class EditorPreferences {

    /// <summary>
    /// The name of the editor preferences file.
    /// </summary>
    public const string PREF_FILE_NAME = "editor_prefs.json";

    // Fonts
    public string FontPath { get; set; } = "Fonts/MPLUS1.ttf";
    public float FontSize { get; set; } = 24.0f;

    // UI geometry styling
    public float WindowRounding { get; set; } = 0f;
    public float FrameRounding { get; set; } = 4f;
    public float GrabRounding { get; set; } = 2f;
    public float GrabMinSize { get; set; } = 12f;
    public float WindowBorderSize { get; set; } = 0.0f;
    public float ScrollbarSize { get; set; } = 15.0f;
    public float IndentSpacing { get; set; } = 20f;
    public Vector2 FramePadding { get; set; } = new(12f, 12f);
    public Vector2 WindowTitleAlign { get; set; } = new(0.05f, 0.5f);
    public Vector2 WindowPadding { get; set; } = new(16f, 16f);
    public Vector2 ItemSpacing { get; set; } = new(12f, 8f);
    public Vector2 WindowMinSize { get; set; } = new(0f, 0f);
    public Vector2 SeparatorTextPadding { get; set; } = new(40f, 25f);

    // Colours
    public Dictionary<string, Vector4> ThemeColors { get; set; } = new() {

        { "WindowBg", new Vector4(0.071f, 0.067f, 0.090f, 1f) },
        { "TitleBg", new Vector4(0.141f, 0.145f, 0.263f, 1f) },
        { "MenuBarBg", new Vector4(0.129f, 0.129f, 0.137f, 1f) },
        { "TitleBgActive", new Vector4(0.259f, 0.141f, 0.263f, 1f) },
        { "Button", new Vector4(0.20f, 0.20f, 0.20f, 1.00f) },
        { "ButtonHovered", new Vector4(0.27f, 0.27f, 0.27f, 1.00f) },
        { "ButtonActive", new Vector4(0.40f, 0.40f, 0.40f, 1.00f) },
        { "HeaderHovered", new Vector4(0.30f, 0.30f, 0.30f, 1.00f) },
        { "HeaderActive", new Vector4(0.35f, 0.35f, 0.35f, 1.00f) },
        { "ScrollBarBg", new Vector4(0.059f, 0.039f, 0.063f, 1f) },
        { "TitleBarBg", new Vector4(0.694f, 0.655f, 0.831f, 1f) },
        { "Header", new Vector4(0.071f, 0.067f, 0.090f, 1f) },
        { "PopupBg", new Vector4(0.129f, 0.129f, 0.137f, 1f) },

    };

    #region Helper Variables
    // All of these should be ignored by the JSON serialisation.

    [JsonIgnore]
    public float TitleBarHeight => MathF.Round(FontSize * 1.5f);

    [JsonIgnore]
    public float MenuBarHeight => MathF.Round(FontSize * 2f);

    [JsonIgnore]
    public float StatusBarHeight => MathF.Round(FontSize * 1.4f);

    [JsonIgnore]
    public float InspectorWidth => MathF.Round(FontSize * 18f);

    [JsonIgnore]
    public float HierarchyWidth => MathF.Round(FontSize * 16f);

    [JsonIgnore]
    public float MenuBarHorizontalSpacing => MathF.Round(FontSize * 0.9f);

    [JsonIgnore]
    public float MenuBarVerticalSpacing => MathF.Round(FontSize * 0.7f);

    [JsonIgnore]
    public float HiearchyVerticalSpacing => MathF.Round(FontSize * 0.8f);

    [JsonIgnore]
    public float HiearchyIndentation => MathF.Round(FontSize);

    #endregion

    /// <summary>
    /// Applies the preferences contained in this class.
    /// </summary>
    /// <param name="style">The ImGui style to apply to.</param>
    public void ApplyTo(ImGuiStylePtr style) {

        // Apply Geometry
        style.WindowRounding = WindowRounding;
        style.FrameRounding = FrameRounding;
        style.GrabRounding = GrabRounding;
        style.WindowBorderSize = WindowBorderSize;
        style.ScrollbarSize = ScrollbarSize;
        style.IndentSpacing = IndentSpacing;
        style.FramePadding = FramePadding;
        style.WindowTitleAlign = WindowTitleAlign;
        style.WindowPadding = WindowPadding;

        // Apply Colors
        foreach (KeyValuePair<string, Vector4> colorKvp in ThemeColors) {

            // Convert the string key (e.g. "WindowBg") back into the ImGuiCol enum
            if (Enum.TryParse<ImGuiCol>(colorKvp.Key, out var imguiColorTarget)) {
                style.Colors[(int)imguiColorTarget] = colorKvp.Value;
            }
        }

    }

    /// <summary>
    /// Loads EditorPreferences from file. If the file doesn't exist, it creates a new EditorPreferences
    /// and saves that to file.
    /// </summary>
    /// <param name="editorRootPath">The path to CorleyEngine Editor's root directory.</param>
    /// <returns>An EditorPreferences instance.</returns>
    public static EditorPreferences LoadOrCreate (string editorRootPath) {

        string prefPath = Path.Combine(editorRootPath, PREF_FILE_NAME);

        Log.Info($"[EditorPreferences] Loading Editor preferences from {prefPath}.");

        if (File.Exists(prefPath)) {

            return DataSerializer.Load<EditorPreferences>(prefPath);

        } else {

            Log.Info($"[EditorPreferences] No Editor preferences file found. Generating default editor_prefs.json...");
            EditorPreferences preferences = new ();
            DataSerializer.Save(preferences, prefPath);
            return preferences;

        }
    }

}
