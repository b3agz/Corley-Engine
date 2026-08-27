using Raylib_cs;
using CorleyEngine.Core;
using System.Numerics;

namespace CorleyEngine;

public class CorelyCursor {
    private Texture2D _texture;
    private Vector2 _hotspot;

    // Primary constructor using a file path
    public CorelyCursor(string texturePath, Vector2 hotspot) {
        _texture = Raylib.LoadTexture(texturePath);
        Raylib.SetTextureFilter(_texture, TextureFilter.Point);
        _hotspot = hotspot;
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

        Vector2 drawPos = new(
            rawMousePos.X - (_hotspot.X * windowScale),
            rawMousePos.Y - (_hotspot.Y * windowScale)
        );

        Raylib.DrawTextureEx(_texture, drawPos, 0f, windowScale, Color.White);
    }

    public void Unload() {
        Raylib.UnloadTexture(_texture);
        Raylib.ShowCursor();
    }
}