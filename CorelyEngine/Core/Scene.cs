using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CorleyEngine.UI;

namespace CorleyEngine.Core;

public class Scene(string name, Camera2D camera) : IDisposable {

    /// <summary>
    /// The name of this scene as it will appear in the editor and be referenced by.
    /// </summary>
    public string Name { get; private set; } = name;

    public Color BackgroundColour { get; private set; } = Color.DarkGray;

    /// <summary>
    /// The camera used to view this scene through.
    /// </summary>
    public Camera2D Camera = camera;

    /// <summary>
    /// The actual player. This gets its own special property because of how important the player is.
    /// </summary>
    public Player Player { get; private set; } = new("./CorelyEngine/Assets/Sprites/default_character.png", new(100, 100));
    // TODO: This would be loaded from file but for now we just need to see things on the screen.

    /// <summary>
    /// A list of objects in this scene.
    /// </summary>
    private List<CorleyObject> _objectsInScene = [];

    public void Init() {

        AddObject(new ProceduralBackground());

        Actor test = new("./CorelyEngine/Assets/Sprites/default_character.png", new(90, 110));

        AddObject(test);
        AddObject(Player);

        TextObject text = new(new Vector2(200, 100), FontSize.Regular);
        text.Text = "Testing Tickles";
        AddObject(text);

    }

    /// <summary>
    /// Adds a CorleyObject to the scene.
    /// </summary>
    public void AddObject(CorleyObject obj) {
        _objectsInScene.Add(obj);
    }

    /// <summary>
    /// Removes a CorleyObject from the scene.
    /// </summary>
    public void RemoveObject(CorleyObject obj) {
        _objectsInScene.Remove(obj);
    }

    public void Dispose() { }

}