using Raylib_cs;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.UI;

public class TextObject : RenderableObject {

    /// <summary>
    /// Relative path to our default font.
    /// </summary>
    private const string FONT_PATH = "./CorelyEngine/Assets/Fonts/Pixelzone.png";

    public override string Name => $"TextObject \"{Text}\"";

    protected Font _font;
    public string Text { get; set; } = "";
    public FontSize FontSize { get; set; } = FontSize.Regular;
    public Color Colour { get; set; } = Color.White;

    /// <summary>
    /// Constructs a new VisualObject.
    /// </summary>
    public TextObject(FontSize fontSize = FontSize.Regular, bool isUI = true) : base(true) {
        _font = LoadFontSafe(FONT_PATH);
        FontSize = fontSize;
    }

    /// <summary>
    /// Constructs a new VisualObject at a specific position.
    /// </summary>
    /// <param name="fontPath">The path to the texture.</param>
    public TextObject(Vector2 position, FontSize fontSize = FontSize.Regular, bool isUI = true) : base(position, isUI) {
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

    /// <summary>
    /// Checks if a point is within the bounds of the text.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is within the bounds, false otherwise.</returns>
    public override bool IsPointInside(Vector2 point) {
        Vector2 size = Raylib.MeasureTextEx(_font, Text, (int)FontSize, 1f);
        // TODO: Account for rotation
        return point.X >= Position.X && point.X <= Position.X + size.X &&
               point.Y >= Position.Y && point.Y <= Position.Y + size.Y;
    }

    /// <summary>
    /// Gets the size of the text.
    /// </summary>
    /// <returns>The size of the text.</returns>
    public override Vector2 GetSize() {
        return Raylib.MeasureTextEx(_font, Text, (int)FontSize, 1f);
    }

    private void InternalDraw() {

        Raylib.DrawTextPro(_font, Text, Position, Vector2.Zero, Rotation, (int)FontSize, 1f, Colour);

    }

}