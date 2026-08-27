using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// Base class for all objects in the game.
/// </summary>
public abstract class CorleyObject : IDisposable {

    public readonly uint Id;
    public string Name { get; set; }

    private Vector2 _position;

    /// <summary>
    /// The position of the object in the game world. Because we are working with pixels,
    /// and because we can't draw half-pixels, this value is automatically rounded to the
    /// nearest whole numbers when set.
    /// </summary>
    public Vector2 Position {
        get => _position;
        set {
            _position = new(MathF.Round(value.X), MathF.Round(value.Y));
        }
    }

    public Vector2 Scale { get; set; } = new (1f, 1f);
    public float Rotation { get; set; } = 0f;
    public uint Depth { get; set; } = 0;
    public bool IsEnabled { get; private set; } = true;

    protected CorleyObject() {
        Id = 0;
        Name = "New CorelyObject";
        Scale = new Vector2(1f, 1f);
        InternalStart();
    }

    protected CorleyObject(Vector2 position) {
        Id = 0;
        Name = "New CorelyObject";
        Position = position;
        Rotation = 0f;
        Scale = new Vector2(1f, 1f);
        InternalStart();
    }

    public void SetEnabled(bool isEnabled) {
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
        OnSubscribeToDraw();
    }

    private void UnsubscribeFromEngineEvents() {
        EngineEvents.OnUpdate -= InternalUpdate;
        OnUnsubscribeFromDraw();
    }

    protected virtual void OnSubscribeToDraw() { }
    protected virtual void OnUnsubscribeFromDraw() { }

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
