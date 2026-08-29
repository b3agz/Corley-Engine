using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;
using CorleyEngine.UI;

namespace CorleyEngine.Game;

public class TestGame : CorleyEngine.Core.Game {
    protected override void OnInitialize() {
        _camera.Position = Vector2.Zero;
        _camera.Offset = Vector2.Zero;
        _camera.Rotation = 0.0f;
        _camera.Zoom = 1.0f;
    }

    protected override void OnLoadContent() {
        SceneManager.ActiveScene = new("Test Scene", _camera);
        SceneManager.ActiveScene.Init();
    }

    protected override void OnUpdate(float deltaTime) {

        // TEMP CODE FOR TESTING
        float moveSpeed = 200f;
        float rotSpeed = 30f;
        float zoomSpeed = 1f;

        if (Input.IsKeyDown(KeyboardKey.W)) _camera.Position -= new Vector2(0, moveSpeed * deltaTime);
        if (Input.IsKeyDown(KeyboardKey.S)) _camera.Position += new Vector2(0, moveSpeed * deltaTime);
        if (Input.IsKeyDown(KeyboardKey.A)) _camera.Position -= new Vector2(moveSpeed * deltaTime, 0);
        if (Input.IsKeyDown(KeyboardKey.D)) _camera.Position += new Vector2(moveSpeed * deltaTime, 0);

        if (Input.IsKeyDown(KeyboardKey.Q)) _camera.Rotation -= rotSpeed * deltaTime;
        if (Input.IsKeyDown(KeyboardKey.E)) _camera.Rotation += rotSpeed * deltaTime;

        if (Input.IsKeyDown(KeyboardKey.T)) _camera.Zoom += zoomSpeed * deltaTime;
        if (Input.IsKeyDown(KeyboardKey.G)) _camera.Zoom -= zoomSpeed * deltaTime;

        if (Input.IsMouseButtonReleased(MouseButton.Left)) {
            if (RenderManager.GetObjectsAtPoint(SceneManager.ActiveScene.Camera.ScreenToWorld(Input.GetVirtualMousePosition()), out List<RenderableObject> objectsAtClick)) {
                foreach(RenderableObject obj in objectsAtClick)
                    Console.WriteLine(obj.Name);
            }
        }
        // END OF TEMP CODE
    }
}
