using Raylib_cs;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.UI;

public class TextObject : RenderableObject {

    /// <summary>
    /// Relative path to our default font.
    /// </summary>
    private const string FONT_PATH = "./CorelyEngine/Assets/Fonts/Pixelzone.png";

    protected Font _font;
    public string Text { get; set; } = "";
    public FontSize FontSize { get; set; } = FontSize.Regular;

    /// <summary>
    /// Constructs a new VisualObject.
    /// </summary>
    public TextObject(FontSize fontSize = FontSize.Regular) : base() {
        _font = LoadFontSafe(FONT_PATH);
        FontSize = fontSize;
    }

    /// <summary>
    /// Constructs a new VisualObject at a specific position.
    /// </summary>
    /// <param name="fontPath">The path to the texture.</param>
    public TextObject(Vector2 position, FontSize fontSize = FontSize.Regular) : base(position) {
        _font = LoadFontSafe(FONT_PATH);
        FontSize = fontSize;
    }

    private Font LoadFontSafe(string path) {
        try {
            return Assets.LoadFont(path);
        }
        catch (Exception) {
            CorleyLog.LogWarning($"Failed to load font at {path}, defaulting to Raylib font.");
            return Raylib.GetFontDefault();
        }
    }

    public override void OnDraw() {
        InternalDraw();
    }

    private void InternalDraw() {

        Raylib.DrawTextPro(_font, Text, Position, Vector2.Zero, Rotation, (int)FontSize, 1f, Color.White);

    }

}