namespace CorleyEngine.Components;

/// <summary>
/// Interface must be implemented by any component.
/// </summary>
public interface IComponent {

    /// <summary>
    /// Runs when the component is initalised at runtime.
    /// </summary>
    void Awake();

    /// <summary>
    /// The logic to be called each frame.
    /// </summary>
    void Update();

    /// <summary>
    /// Runs any time the component is modified in the editor.
    /// </summary>
    void OnChange();

}