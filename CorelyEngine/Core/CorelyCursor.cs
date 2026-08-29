using Raylib_cs;
using CorleyEngine.Core;
using System.Numerics;

namespace CorleyEngine;

public class CorelyCursor {

    /// <summary>
    /// Relative path to the cursor image file.
    /// </summary>
    private const string CURSOR_PATH = "./CorelyEngine/Assets/Sprites/default_cursor.png";

    private Texture2D _texture;
    private Vector2 _hotspot;

    // Primary constructor using a file path
    public CorelyCursor() {
        _texture = Assets.LoadTexture(CURSOR_PATH);
        Raylib.SetTextureFilter(_texture, TextureFilter.Point);
        _hotspot = new (2, 2);
        Raylib.HideCursor();
    }

    // Fallback constructor for the temporary generated texture
    public CorelyCursor(Texture2D generatedTexture, Vector2 hotspot) {
        _texture = generatedTexture;
        Raylib.SetTextureFilter(_texture, TextureFilter.Point);
        _hotspot = hotspot;
        Raylib.HideCursor();
    }

    public void Update(float deltaTime) {
        // TODO: Gamepad handling, animation state timers
    }

    public void Draw(Vector2 rawMousePos, float windowScale) {

        // Calculate position of mouse in window, make sure it's rounded to avoid pixel shimmering.
        Vector2 drawPos = new(
            MathF.Round(rawMousePos.X - (_hotspot.X * windowScale)),
            MathF.Round(rawMousePos.Y - (_hotspot.Y * windowScale))
        );

        Raylib.DrawTextureEx(_texture, drawPos, 0f, windowScale, Color.White);
    }

    public void Unload() {
        Raylib.UnloadTexture(_texture);
        Raylib.ShowCursor();
    }
}