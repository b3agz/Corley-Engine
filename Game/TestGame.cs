using System.Numerics;
using CorleyEngine.Core;
using CorleyEngine.UI;

namespace CorleyEngine.Game;

public class TestGame : CorleyEngine.Core.Game {
    protected override void OnInitialize() {
        _camera.Target = Vector2.Zero;
        _camera.Offset = Vector2.Zero;
        _camera.Rotation = 0.0f;
        _camera.Zoom = 1.0f;
    }

    protected override void OnLoadContent() {
        SceneManager.ActiveScene = new("Test Scene", _camera);
        SceneManager.ActiveScene.Init();
    }

    protected override void OnUpdate(float deltaTime) {
        // This is where the game's core logic would go. Anything that is game-wide and needs to run every frame
        // should go here.
    }
}
