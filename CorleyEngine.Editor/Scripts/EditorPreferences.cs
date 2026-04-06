using System;
using System.Collections.Generic;
using System.Numerics;

namespace CorleyEngine.Editor;

/// <summary>
/// A class for storing the editor's preferences, designed to be easily serialisable for storage.
/// </summary>
public class EditorPreferences {

    // Fonts
    public string FontPath { get; set; } = "Fonts/MPLUS1.ttf";
    public float FontSize { get; set; } = 22.0f;

    // UI geometry styling
    public float WindowRounding { get; set; } = 10.0f;
    public float FrameRounding { get; set; } = 6.0f;
    public float GrabRounding { get; set; } = 6.0f;
    public float WindowBorderSize { get; set; } = 0.0f;
    public float ScrollbarSize { get; set; } = 15.0f;
    public Vector2 FramePadding { get; set; } = new(5.0f, 5.0f);
    public Vector2 WindowTitleAlign { get; set; } = new(0.5f, 0.5f);
    public Vector2 WindowPadding { get; set; } = new(5.0f, 5.0f);

    // Colours
    public Dictionary<string, Vector4> ThemeColors { get; set; } = new() {

        { "WindowBg", new Vector4(0.12f, 0.12f, 0.12f, 1.00f) },
        { "TitleBg", new Vector4(0.08f, 0.08f, 0.08f, 1.00f) },
        { "TitleBgActive", new Vector4(0.16f, 0.16f, 0.16f, 1.00f) },
        { "Button", new Vector4(0.20f, 0.20f, 0.20f, 1.00f) },
        { "ButtonHovered", new Vector4(0.27f, 0.27f, 0.27f, 1.00f) },
        { "ButtonActive", new Vector4(0.40f, 0.40f, 0.40f, 1.00f) },
        { "Header", new Vector4(0.25f, 0.25f, 0.25f, 1.00f) },
        { "HeaderHovered", new Vector4(0.30f, 0.30f, 0.30f, 1.00f) },
        { "HeaderActive", new Vector4(0.35f, 0.35f, 0.35f, 1.00f) }

    };
}
