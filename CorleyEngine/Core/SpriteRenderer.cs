using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorleyEngine.Components;

/// <summary>
/// Renders a <see cref="Texture2D"/> to the screen.
/// </summary>
/// <param name="texture">The Texture2D to be rendered.</param>
public class SpriteRenderer(Texture2D texture) : IComponent, IDrawableComponent {

    /// <summary>
    /// The texture that is being rendered as a sprite.
    /// </summary>
    public Texture2D Texture { get; set; } = texture;

    /// <summary>
    /// Colour tint to be applied to the sprite.
    /// </summary>
    public Color Tint { get; set; } = Color.White;

    /// <inheritdoc />
    public int DrawOrder { get; set; } = 0;

    /// <summary>
    /// The pivot point the sprite being rendered.
    /// </summary>
    public Vector2 Origin { get; set; } = Vector2.Zero;

    public void Update() {
        // SpriteRenderer doesn't need an Update function but must have one to satisfy IComponent.
    }

    /// <inheritdoc />
    void IDrawableComponent.Draw(SpriteBatch spriteBatch, Transform transform) {

        spriteBatch.Draw(
            Texture,
            transform.Position,
            null,
            Tint,
            transform.Rotation,
            Origin,
            transform.Scale,
            SpriteEffects.None,
            0f
        );
    }
}
