using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;
using Microsoft.Xna.Framework;

namespace CorleyEngine.Core;

/// <summary>
/// Manages a collection of Entities, handling their Update and Draw cycles.
/// </summary>
public class Scene {

    /// <summary>
    /// The runtime scene holds the pure data and reads the information from there.
    /// </summary>
    public SceneData Data { get; private set; }

    private readonly List<Entity> _entities = [];

    /// <summary>
    /// The camera that is currently viewing this scene.
    /// </summary>
    public Camera MainCamera { get; set; }

    /// <summary>
    /// Binds the raw scene data to this runtime scene instance.
    /// </summary>
    public void Initialize(SceneData blueprint) {

        Data = blueprint;

        foreach (Entity loadedEntity in blueprint.Entities) {

            loadedEntity.OnAfterDeserialize();
            AddEntity(loadedEntity);

        }
    }


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

        // 1. Get the camera's matrix (or use Identity if no camera exists, which means no offset)
        Matrix viewMatrix = Camera.MainCamera != null ? Camera.MainCamera.GetViewMatrix() : Matrix.Identity;

        // 2. Begin the batch using the camera's illusion!
        // Note: SpriteSortMode.Deferred and BlendState.AlphaBlend are MonoGame defaults.
        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            null, null, null, null,
            viewMatrix // <--- The magic happens here
        );

        foreach (Entity entity in _entities) {
            entity.Draw(spriteBatch);
        }

        spriteBatch.End();
    }
}