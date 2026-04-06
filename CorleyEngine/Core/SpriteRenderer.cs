using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;
using CorleyEngine.Core;

namespace CorleyEngine.Components;

/// <summary>
/// Renders a <see cref="Texture2D"/> to the screen.
/// </summary>
/// <param name="texture">The Texture2D to be rendered.</param>
/// <remarks>
/// We can't serialise a texture file, so we tell the serialiser to ignore the texture file and
/// instead we store the path to the texture. Whenever that path is changed, the SpriteRenderer
/// automatically changes the texture. This way, we can serialise the SpriteRenderer as text.
/// </remarks>
public class SpriteRenderer : Component, IDrawableComponent {

    // Tell the serializer that we want to store the contents of this field.
    [JsonInclude]
    [JsonPropertyName("TexturePath")]
    private string _texturePath;

    /// <summary>
    /// The relative path to the texture in the project assets folder.
    /// </summary>
    [JsonIgnore]
    public string TexturePath {
        get => _texturePath;
        set {
            _texturePath = value;
            HotSwapTexture();
        }
    }

    /// <summary>
    /// The texture that is being rendered as a sprite.
    /// </summary>
    [JsonIgnore]
    public Texture2D Texture { get; set; }

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

    /// <summary>
    /// Creates a blank SpriteRenderer component.
    /// </summary>
    public SpriteRenderer() { }

    /// <summary>
    /// Creates a SpriteRenderer component with the Texture2D stored at <paramref name="texturePath"/>.
    /// </summary>
    /// <param name="texturePath">Path to the texture file relative to the project's asset folder.</param>
    public SpriteRenderer(string texturePath) {
        TexturePath = texturePath;
    }

    /// <inheritdoc />
    public override void Awake() {
        HotSwapTexture();
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

    private void HotSwapTexture() {

        if (!string.IsNullOrWhiteSpace(_texturePath)) {

            Texture = AssetManager.Get<Texture2D>(_texturePath);

            if (Origin == Vector2.Zero && Texture != null) {
                Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            }
        }
        else {
            Texture = null;
        }

    }

}
