using Microsoft.Xna.Framework.Graphics;

namespace CorleyEngine.Components;

/// <summary>
/// Interface must be implemented by any component that draws something to the screen.
/// </summary>
public interface IDrawableComponent : IComponent {

    /// <summary>
    /// Dictates order things are drawn to the screen. Lower numbers are drawn first.
    /// </summary>
    int DrawOrder { get; set; }

    /// <summary>
    /// Draws a <see cref="SpriteBatch"/> to the screen.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="transform"></param>
    void Draw(SpriteBatch spriteBatch, Transform transform);

}