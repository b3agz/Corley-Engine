using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// Base class for all objects in the game.
/// </summary>
public abstract class CorleyObject : IDisposable {

    public readonly uint Id;
    public virtual string Name { get; set; }
    public bool IsEnabled { get; private set; } = true;

    protected CorleyObject() {
        Id = 0;
        Name = "New CorelyObject";
        InternalStart();
    }

    protected CorleyObject(Vector2 position) {
        Id = 0;
        Name = "New CorelyObject";
        InternalStart();
    }

    public virtual void SetEnabled(bool isEnabled) {
        if (isEnabled == IsEnabled)
            return;

        IsEnabled = isEnabled;
        if (IsEnabled)
            SubscribeToEngineEvents();
        else
            UnsubscribeFromEngineEvents();
    }

    private void SubscribeToEngineEvents() {
        EngineEvents.OnUpdate += InternalUpdate;
    }

    private void UnsubscribeFromEngineEvents() {
        EngineEvents.OnUpdate -= InternalUpdate;
    }

    private void InternalStart() {
        SubscribeToEngineEvents();
        OnStart();
    }

    public virtual void OnStart() { }

    private void InternalUpdate(float deltaTime) => OnUpdate(deltaTime);

    /// <summary>
    /// Called each frame on any objects that are active in the scene. Handles logic only (no rendering).
    /// </summary>
    /// <param name="deltaTime">The amount of time in seconds that has elapsed since the last frame.</param>
    public virtual void OnUpdate(float deltaTime) { }

    public void Destroy() => Dispose();
    public void Dispose() {
        UnsubscribeFromEngineEvents();
        OnDestroy();
        GC.SuppressFinalize(this);
    }

    internal virtual void OnDestroy() { }
}
