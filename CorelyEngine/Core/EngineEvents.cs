using System;

namespace CorleyEngine.Core;

/// <summary>
/// A global static class providing access to critical engine events.
/// </summary>
public static class EngineEvents {

    /// <summary>
    /// The logic update, invoked every frame for anything with an Update loop.
    /// </summary>
    public static event Action<float>? OnUpdate;

    /// <summary>
    /// The render update invoked every frame for anything with something to draw.
    /// </summary>
    public static event Action? OnDraw;

    /// <summary>
    /// Broadcasts the logic tick to all listeners.
    /// </summary>
    public static void TickUpdate(float deltaTime) => OnUpdate?.Invoke(deltaTime);

    /// <summary>
    /// Broadcasts the render tick to all listeners.
    /// </summary>
    public static void TickDraw() => OnDraw?.Invoke();

    /// <summary>
    /// Wipes all subscriptions. Call this when transitioning between rooms.
    /// </summary>
    public static void ClearAll() {
        OnUpdate = null;
        OnDraw = null;
    }
}