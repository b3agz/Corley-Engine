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
    /// Whether this Entity is currently active in the scene.
    /// </summary>
    public bool IsActive { get; set; } = true;

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

    /// <summary>
    /// A list of all the <see cref="IComponent"/>s attached to this Entity.
    /// </summary>
    [JsonInclude]
    [JsonPropertyName("Components")]
    public List<IComponent> Components {
        get => _components;
        private set => _components = value ?? [];
    }
    private List<IComponent> _components = [];

    #region Scene Hierarchy


    /// <summary>
    /// The parent of the entity. If null, the entity is in the root of the scene.
    /// </summary>
    /// <remarks>
    /// Parent is excluded from JSON serialisation to avoid circular referencing. We store the children
    /// and reconstruct the relationship between them afterwards.
    /// </remarks>
    [JsonIgnore]
    public Entity Parent { get; private set; }

    /// <summary>
    /// Sets a new parent of this entity.
    /// </summary>
    /// <param name="newParent">The <see cref="Entity"/> that is to be the new parent.</param>
    /// <remarks>
    /// This method handles the setting of a new parent in both directions; setting the new parent and
    /// unsetting the previous parents. This is necessary to prevent circular referencing particularly
    /// in JSON serialisation. Entity's never remove their own children.
    /// </remarks>
    public void SetParent(Entity newParent) {

        // If the new parent is the same as the current one, we don't need to do anything.
        if (newParent == Parent)
            return;

        // Prevent an endless cycle caused by setting a parent that is currently a child.
        if (newParent != null && newParent.IsDescendantOf(this))
            return;

        // If the entity already has a parent, we need to tell that parent that this one is no longer a child.
        Parent?._children.Remove(this);

        // Set the parent to the new parent. Parent should be null if the entity is not a child of anyone so we
        // don't need to check if newParent is null.
        Parent = newParent;

        // If we just set the parent to null, we can break out here.
        if (Parent == null)
            return;

        // Tell the new parent that it has a new child.
        if (!Parent._children.Contains(this))
            Parent._children.Add(this);

    }

    /// <summary>
    /// Re-establishes the parent/child links between this entity and any children it has.
    /// </summary>
    /// <remarks>
    /// Moves recurisively down the hierarchy from this entity until there are no more children.
    /// </remarks>
    public void RelinkChildren() {

        foreach (Entity child in Children) {
            child.Parent = this;
            child.RelinkChildren();
        }

    }

    /// <summary>
    /// Checks if this entity is a descendent of <paramref name="potentialAncestor"/> in the hiearchy.
    /// </summary>
    /// <param name="potentialAncestor">The entity that is potentially higher in the hiearchy.</param>
    /// <returns>Returns true if this entity is a descendent of potentialAncestor.</returns>
    public bool IsDescendantOf(Entity potentialAncestor) {

        if (potentialAncestor == null) return false;

        // Start with this entity's parent.
        Entity current = this.Parent;

        // Crawl up the hierarchy until we encounter the potentialAncestor.
        while (current != null) {

            if (current == potentialAncestor) {
                return true;
            }
            current = current.Parent;
        }

        // If we get here, we're at the root and we didn't encounter potentialAnchor, we can return false.
        return false;
    }

    /// <summary>
    /// A list of all the children of this entity.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<Entity> Children => _children;

    [JsonInclude]
    [JsonPropertyName("Children")]
    private List<Entity> _children = [];

    #endregion

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