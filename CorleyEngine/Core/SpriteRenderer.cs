using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorleyEngine.Components;

/// <summary>
/// Renders a <see cref="Texture2D"/> to the screen.
/// </summary>
/// <param name="texture">The Texture2D to be rendered.</param>
public class SpriteRenderer(Texture2D texture) : IComponent, IDrawableComponent {

    public Texture2D Texture { get; set; } = texture;
    public Color Tint { get; set; } = Color.White;
    public int DrawOrder { get; set; } = 0;

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
            Vector2.Zero,
            transform.CurrentScale,
            SpriteEffects.None,
            0f
        );
    }
}
