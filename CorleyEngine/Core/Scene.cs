using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace CorleyEngine.Core;

/// <summary>
/// Manages a collection of Entities, handling their Update and Draw cycles.
/// </summary>
public class Scene {

    private readonly List<Entity> _entities = [];

    /// <summary>
    /// Adds an Entity to the scene.
    /// </summary>
    public void AddEntity(Entity entity) {
        _entities.Add(entity);
    }

    /// <summary>
    /// Removes an Entity and releases its ID back to the registry.
    /// </summary>
    public void RemoveEntity(Entity entity) {
        if (_entities.Remove(entity)) {
            EntityRegistry.ReleaseId(entity.Id);
        }
    }

    /// <summary>
    /// Updates every Entity in the scene.
    /// </summary>
    public void Update() {

        // Loop backwards so any entities that get removed in the course of the update don't
        // break the iteration.
        for (int i = _entities.Count - 1; i >= 0; i--) {
            _entities[i].Update();
        }
    }

    /// <summary>
    /// Draws every "OnStage" Entity in the scene.
    /// </summary>
    public void Draw(SpriteBatch spriteBatch) {
        foreach (Entity entity in _entities) {
            entity.Draw(spriteBatch);
        }
    }
}