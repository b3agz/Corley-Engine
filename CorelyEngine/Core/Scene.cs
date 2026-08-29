using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using CorleyEngine.UI;

namespace CorleyEngine.Core;

public class Scene(string name, Camera camera) : IDisposable {

    /// <summary>
    /// The name of this scene as it will appear in the editor and be referenced by.
    /// </summary>
    public string Name { get; private set; } = name;

    public Color BackgroundColour { get; private set; } = Color.DarkGray;

    /// <summary>
    /// The camera used to view this scene through.
    /// </summary>
    public Camera Camera = camera;

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

        //AddObject(new ProceduralBackground());

        Actor test = new("./CorelyEngine/Assets/Sprites/default_character.png", new(490, 110));
        test.Colour = Color.Pink;
        test.FlipX = true;
        test.HintText = "Frank";

        Actor test2 = new("./CorelyEngine/Assets/Sprites/default_character.png", new(250, 200));
        test2.Colour = Color.Green;
        test2.FlipY = true;
        test2.HintText = "Jim";

        AddObject(test);
        AddObject(Player);

        TextObject text = new(new Vector2(200, 100), FontSize.Large, false);
        text.Text = "Testing Tickles";
        text.Colour = Color.DarkBlue;
        AddObject(text);

        AddObject(new HintObject());

        TextObject uitext = new(new Vector2(10, 10), FontSize.Regular);
        uitext.Text = "UI Text Stuff";
        uitext.Colour = Color.White;
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