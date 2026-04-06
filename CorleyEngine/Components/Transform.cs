using Microsoft.Xna.Framework;

namespace CorleyEngine.Components;

/// <summary>
/// Holds the position, rotation, and scale of an <see cref="Entity"/> in a <see cref="Scene"/>.
/// Every entity that is not a Director must have a transform.
/// </summary>
public class Transform : IComponent {

    /// <summary>
    /// The position of this Transform in the <see cref="Scene"/>.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The rotation of this Transform in radians.
    /// </summary>
    public float Rotation { get; set; } = 0f;

    /// <summary>
    /// The scale of this Transform as it is when brought into the scene (before any
    /// perspective-based modification).
    /// </summary>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// Creates a new Transform at a specific postion.
    /// </summary>
    public Transform(Vector2 startPosition) {
        Position = startPosition;
    }

    /// <summary>
    /// Creates a new Transform with a position of 0, 0.
    /// </summary>
    public Transform() {
        Position = Vector2.Zero;
    }

    /// <inheritdoc />
    void IComponent.Awake() { }

    /// <inheritdoc />
    void IComponent.Update() { }

    /// <inheritdoc />
    public void OnChange() { }

}

