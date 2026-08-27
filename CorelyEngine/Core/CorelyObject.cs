using System.Numerics;

namespace CorleyEngine.Core;

public abstract class CorleyObject : IDisposable {

    public readonly uint Id;

    public string Name { get; set; }

    /// <summary>
    /// Whether this object is "on stage" (has a visual presence). Cannot be changed once initialised.
    /// </summary>
    public readonly bool OnStage;

    /// <summary>
    /// The position of this object in the scene.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The sale of this object relative to its default.
    /// </summary>
    public Vector2 Scale { get; set; }

    /// <summary>
    /// Essentially sorting order or draw order. Determines the order that things are drawn in
    /// on the scene. The higher the number, the further away from the camera.
    /// </summary>
    /// <remakrs>
    /// Depth is unsigned as there is no reason to ever have a negative depth.
    /// </remarks>
    public uint Depth { get; set; }

    /// <summary>
    /// Shows whether the object is currently active in the scene.
    /// </summary>
    public bool IsEnabled { get; private set; } = true;

    /// <summary>
    /// Sets whether this object is active or not. When not active, loop methods
    /// (such as Update()) will not be called.
    /// </summary>
    /// <param name="isEnabled">True if active, false if not.</param>
    public void SetEnabled(bool isEnabled) {

        // If enabled is being set to the same value, we don't want to subscribe to events twice
        // or attempt to unsubscribe when we're not subscribed.
        if (isEnabled == IsEnabled)
            return;

        IsEnabled = isEnabled;
        if (IsEnabled)
            SubscribeToEngineEvents();
        else
            UnsubscribeFromEngineEvents();

    }

    /// <summary>
    /// Loop events are made up of an internal hidden method and an abstract public method.
    /// The internal methods are the ones that subscribe to the events and call the public
    /// methods. This allows for non-negotiable logic to be included in this parent class
    /// without having to worry about it in the child classes.
    /// </summary>
    #region Loop Event Classes

    /// <summary>
    /// Subscribes this object to any engine events it needs to subscribe to.
    /// </summary>
    private void SubscribeToEngineEvents() {
        EngineEvents.OnUpdate += InternalUpdate;
        if (OnStage)
            EngineEvents.OnDraw += InternalDraw;
    }

    /// <summary>
    /// Unsubscribes this object to any engine events it was subscribed to.
    /// </summary>
    private void UnsubscribeFromEngineEvents() {
        EngineEvents.OnUpdate -= InternalUpdate;
        if (OnStage)
            EngineEvents.OnDraw -= InternalDraw;
    }

    /// <summary>
    /// Called when the object is created.
    /// </summary>
    private void InternalStart() {
        SubscribeToEngineEvents();
        OnStart();
    }

    /// <summary>
    /// Called once when the object is first loaded into the scene.
    /// </summary>
    public virtual void OnStart() { }

    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="deltaTime">The amount of time that has passed since the last update.</param>
    private void InternalUpdate(float deltaTime) {
        OnUpdate(deltaTime);
    }

    /// <summary>
    /// Called every frame on active objects. Logic only.
    /// </summary>
    public virtual void OnUpdate(float deltaTime) { }

    /// <summary>
    /// Called every frame to handle rendering.
    /// </summary>
    /// <remarks>
    /// Does not need a check to make sure this object is OnStage as the draw method will not be subscribed to for off stage objects.
    /// </remarks>
    private void InternalDraw() {
        OnDraw();
    }

    /// <summary>
    /// Called every frame on active objects that have a physical presence in the scene.
    /// Render logic only.
    /// </summary>
    public virtual void OnDraw() { }

    #endregion

    #region Object Destruction

    /// <summary>
    /// Destroys this object.
    /// </summary>
    public void Destroy() {
        Dispose();
    }

    public void Dispose() {
        UnsubscribeFromEngineEvents();
        OnDestroy();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Called when the object is destroyed.
    /// </summary>
    internal virtual void OnDestroy() { }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructs a new CorelyObject.
    /// </summary>
    /// <param name="isOnStage">Determines if the CorelyObject is on stage. If yes, Position defaults to 0,0.</param>
    public CorleyObject(bool isOnStage) {
        Id = 0;
        Name = "New CorelyObject";
        OnStage = isOnStage;
        InternalStart();
    }

    /// <summary>
    /// Constructs a new OnStage CorelyObject at <paramref name="position"/>
    /// </summary>
    /// <param name="position">The position the new CorelyObject is created at.</param>
    /// <remarks>
    /// CorelyObjects that are not OnStage do not need a position, so in passing in a position to the constructor,
    /// we can assume this CorelyObject is OnStage.
    /// </remarks>
    public CorleyObject(Vector2 position) {
        Id = 0;
        Name = "New CorelyObject";
        Position = position;
        InternalStart();
    }

    #endregion

}