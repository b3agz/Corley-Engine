using Raylib_cs;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.UI;

public class TextObject : CorleyObject {

    protected Font? _font;
    private readonly string _fontPath;
    public string Text { get; set; } = "";
    public float FontSize { get; set; } = 20f;

    /// <summary>
    /// Constructs a new VisualObject.
    /// </summary>
    /// <param name="fontPath">The path to the texture.</param>
    public TextObject(string fontPath) : base() {
        _fontPath = fontPath;
        _font = LoadFontSafe(fontPath);
    }

    /// <summary>
    /// Constructs a new VisualObject at a specific position.
    /// </summary>
    /// <param name="fontPath">The path to the texture.</param>
    /// <param name="position">The position of the object.</param>
    public TextObject(string fontPath, Vector2 position) : base(position) {
        _fontPath = fontPath;
        _font = LoadFontSafe(fontPath);
    }

    protected override void OnSubscribeToDraw() {
        // TODO: UI Render Manager
    }

    protected override void OnUnsubscribeFromDraw() {
        // TODO: UI Render Manager
    }

    private Font? LoadFontSafe(string path) {
        try {
            return Assets.LoadFont(path);
        }
        catch (Exception) {
            CorleyLog.LogWarning($"Failed to load font at {path}");
            return null;
        }
    }

    public void OnDraw() {
        InternalDraw();
    }

    private void InternalDraw() {

        // Draw the image to the screen. If the image was never found, draw an eye-searing magenta square
        // so we know it went wrong.
        if (_font.HasValue) {
            Raylib.DrawTextEx(_font.Value, Text, Position, FontSize, 1f, Color.White);
        }
        else {
            Raylib.DrawRectanglePro(new Rectangle(Position.X, Position.Y, 32 * Scale.X, 32 * Scale.Y), Vector2.Zero, Rotation, Color.Magenta);
        }
    }

}