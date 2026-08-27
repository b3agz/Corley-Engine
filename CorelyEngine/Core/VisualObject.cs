using System;
using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// A base class for objects that have a visual presence in the scene.
/// </summary>
public abstract class VisualObject : CorleyObject {

    protected Texture2D? _texture;
    private readonly string _texturePath;

    /// <summary>
    /// Constructs a new VisualObject.
    /// </summary>
    /// <param name="texturePath">The path to the texture.</param>
    public VisualObject(string texturePath) : base() {
        _texturePath = texturePath;
        _texture = LoadTextureSafe(texturePath);
    }

    /// <summary>
    /// Constructs a new VisualObject at a specific position.
    /// </summary>
    /// <param name="texturePath">The path to the texture.</param>
    /// <param name="position">The position of the object.</param>
    public VisualObject(string texturePath, Vector2 position) : base(position) {
        _texturePath = texturePath;
        _texture = LoadTextureSafe(texturePath);
    }

    protected override void OnSubscribeToDraw() {
        EngineEvents.OnDraw += InternalDraw;
    }

    protected override void OnUnsubscribeFromDraw() {
        EngineEvents.OnDraw -= InternalDraw;
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

    private void InternalDraw() {

        // Draw the image to the screen. If the image was never found, draw an eye-searing magenta square
        // so we know it went wrong.
        if (_texture.HasValue && _texture.Value.Id != 0) {
            Raylib.DrawTextureV(_texture.Value, Position, Color.White);
        }
        else {
            Raylib.DrawRectangle((int)Position.X, (int)Position.Y, 32, 32, Color.Magenta);
        }
    }
}
