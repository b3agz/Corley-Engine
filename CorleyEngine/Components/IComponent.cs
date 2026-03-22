namespace CorleyEngine.Components;

/// <summary>
/// Interface must be implemented by any component.
/// </summary>
public interface IComponent {

    /// <summary>
    /// The logic to be called each frame.
    /// </summary>
    void Update();

}