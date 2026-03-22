using CorleyEngine.Core;

namespace CorleyEngine.Components;

/// <summary>
/// A base class that user-made components derive from. Component provides life
/// cycle functions, such as <see cref="Awake"/>, and <see cref="Update"/>.
/// </summary>
public abstract class Component : IComponent {

    /// <summary>
    /// The Entity this component is currently attached to.
    /// </summary>
    /// <remarks>The setter is internal so only the Entity can assign this.</remarks>
    public Entity Entity { get; internal set; }

    /// <summary>
    /// Called once when the component is added to an Entity.
    /// </summary>
    public virtual void Awake() { }

    /// <summary>
    /// Logic to run every frame.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    /// Satisfies IComponnent without hiding the Update method from the user.
    /// </summary>
    void IComponent.Update() => Update();
}
