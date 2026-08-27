using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace CorleyEngine.Core;

public class Scene(string name, Camera2D camera) : IDisposable {

    /// <summary>
    /// The name of this scene as it will appear in the editor and be referenced by.
    /// </summary>
    public string Name { get; private set; } = name;

    /// <summary>
    /// The camera used to view this scene through.
    /// </summary>
    public Camera2D Camera  = camera;

    /// <summary>
    /// A list of objects in this scene.
    /// </summary>
    private List<CorleyObject> _objectsInScene = [];

    public void Init() {

        // TODO: This would be loaded from file but for now we just need to see things on the screen.
        Actor actor = new Actor("./Sprites/default_character.png", new (100, 100));
        _objectsInScene.Add(actor);

    }

    public void Dispose() { }

}