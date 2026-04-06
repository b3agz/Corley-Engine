using System.Collections.Generic;
using CorleyEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace CorleyEngine.Core;

/// <summary>
/// Creates a new Entity instance.
/// </summary>
public class Entity(string name) {

    /// <summary>
    /// The ID of this Entity. Each Entity's ID must be unique within the project that contains it.
    /// </summary>
    public int Id { get; private set; } = EntityRegistry.GenerateId();

    /// <summary>
    /// The name of the Entity.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Returns true if the Entity is physically in the scene (ie, has a <see cref="Components.Transform"/>).
    /// </summary>
    [JsonIgnore]
    public bool IsOnStage => _transform != null;

    /// <summary>
    /// Returns true if the Entity is not physically in the scene (ie, does NOT have a <see cref="Components.Transform"/>).
    /// </summary>
    [JsonIgnore]
    public bool IsDirector => _transform == null;

    // A list of all of the components attached to this Entity.
    [JsonInclude]
    [JsonPropertyName("Components")]
    public List<IComponent> Components {
        get => _components;
        private set => _components = value ?? new List<IComponent>();
    }

    // This stays completely hidden from the serializer now
    private List<IComponent> _components = [];

    /// Returns the attached Transform". Null if Entity is a Director. Because the presence (or not)
    /// of a Transform is used to determine whether this Entity is a Director or IsOnStage, caching
    /// it removes the need to check the component list every time.
    private Transform _transform;

    [JsonIgnore]
    public Transform Transform => _transform;

    /// <summary>
    /// Creates a new "OnStage" Entity (has a <see cref="Transform"/> already attached).
    /// </summary>
    /// <param name="name">The name of the Entity.</param>
    /// <param name="position">Where in the <see cref="Scene"/> the Entity should be spawned.</param>
    /// <returns>A new Stage Entity.</returns>
    public static Entity CreateStageEntity(string name, Vector2 position) {

        Entity entity = new Entity(name);
        entity.AddComponent(new Transform(position));
        return entity;

    }

    /// <summary>
    /// Creates a new "Director" Entity (no <see cref="Transform"/> attached.)
    /// </summary>
    /// <param name="name">The name of the Entity.</param>
    /// <returns>A new Director Entity</returns>
    public static Entity CreateDirector(string name) {
        return new Entity(name);
    }

    /// <summary>
    /// Adds a new component to the Entity.
    /// </summary>
    /// <param name="component">The component instance to attach.</param>
    /// <remarks>
    /// Only one <see cref="Components.Transform"/> is permitted per Entity. Adding a second Transform will be ignored.
    /// </remarks>
    public void AddComponent(IComponent component) {

        // TODO: Checks for conflicting components. Eg, you can't have more than one transform.

        // If the component is a transform and we already have one, we can't add another. If we don't
        // have one, we need to set the transform reference to it.
        if (component is Transform transform) {
            if (_transform != null) return;
            _transform = transform;
        } else if (component is Component c) {
            c.Entity = this;
            c.Awake();
        }

        _components.Add(component);
    }

    /// <summary>
    /// Retrieves the first component of type T attached to this Entity.
    /// </summary>
    public T GetComponent<T>() where T : class, IComponent {

        foreach (IComponent component in _components)
            if (component is T result) return result;

        return null;

    }

    /// <summary>
    /// Called every frame on active Entities.
    /// </summary>
    public void Update() {

        // The Entity itself doesn't need any logic, it only needs to tell all of its attched components
        // to run their Update logic.
        foreach (IComponent component in _components)
            component.Update();

    }

    /// <summary>
    /// Draws this entity to the screen.
    /// </summary>
    /// <param name="spriteBatch">The <see cref="SpriteBatch"/> being drawn.</param>
    /// <remarks>Returns without doing anything if Entity is a Director.</remarks>
    public void Draw(SpriteBatch spriteBatch) {

        if (IsDirector) return;

        // Find any components that implement the IDrawableComponent interface and call their Draw method.
        foreach (IComponent component in _components) {

            if (component is IDrawableComponent drawable)
                drawable.Draw(spriteBatch, _transform);

            // TODO: Separate IDrawableComponents into a separate list so we don't have to check every component.

        }
    }

    /// <summary>
    /// Called by the engine immediately after this Entity is loaded from a JSON file.
    /// Re-establishes parent references, caches the transform, and triggers Awake().
    /// </summary>
    public void OnAfterDeserialize() {

        foreach (IComponent component in _components) {

            if (component is Transform t) {
                _transform = t;
            }
            else if (component is Component c) {
                c.Entity = this;
                c.Awake();
            }
        }
    }
}