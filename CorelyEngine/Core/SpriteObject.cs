using System;
using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// A base class for objects that render a sprite (texture) to the screen.
/// </summary>
public abstract class SpriteObject : RenderableObject {

    protected Texture2D? _texture;
    private readonly string _texturePath;

    /// <summary>
    /// Constructs a new VisualObject.
    /// </summary>
    /// <param name="texturePath">The path to the texture.</param>
    public SpriteObject(string texturePath) : base() {
        Name = "SpriteObject";
        _texturePath = texturePath;
        _texture = LoadTextureSafe(texturePath);
    }

    /// <summary>
    /// Constructs a new VisualObject at a specific position.
    /// </summary>
    /// <param name="texturePath">The path to the texture.</param>
    /// <param name="position">The position of the object.</param>
    public SpriteObject(string texturePath, Vector2 position) : base(position) {
        Name = "SpriteObject";
        _texturePath = texturePath;
        _texture = LoadTextureSafe(texturePath);
    }

    private Texture2D? LoadTextureSafe(string path) {
        try {
            return Assets.LoadTexture(path);
        }
        catch (Exception) {
            CorleyLog.LogWarning($"Failed to load texture at {path}");
            return null;
        }
    }

    public override void OnDraw() {
        InternalDraw();
    }

    /// <summary>
    /// Checks if a point is within the bounds of the sprite.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is within the bounds, false otherwise.</returns>
    public override bool IsPointInside(Vector2 point) {
        if (_texture.HasValue && _texture.Value.Id != 0) {
            float width = _texture.Value.Width * Scale.X;
            float height = _texture.Value.Height * Scale.Y;
            // TODO: Account for rotation
            return point.X >= Position.X && point.X <= Position.X + width &&
                   point.Y >= Position.Y && point.Y <= Position.Y + height;
        }
        return false;
    }

    private void InternalDraw() {

        // Draw the image to the screen. If the image was never found, draw an eye-searing magenta square
        // so we know it went wrong.
        if (_texture.HasValue && _texture.Value.Id != 0) {
            Rectangle sourceRec = new(0, 0, _texture.Value.Width, _texture.Value.Height);
            Rectangle destRec = new(Position.X, Position.Y, _texture.Value.Width * Scale.X, _texture.Value.Height * Scale.Y);

            // Draw using origin at 0,0 to match Position as top-left corner
            Raylib.DrawTexturePro(_texture.Value, sourceRec, destRec, Vector2.Zero, Rotation, Color.White);
        }
        else {
            Raylib.DrawRectanglePro(new Rectangle(Position.X, Position.Y, 32 * Scale.X, 32 * Scale.Y), Vector2.Zero, Rotation, Color.Magenta);
        }
    }
}
