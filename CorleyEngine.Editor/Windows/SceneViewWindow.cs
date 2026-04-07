using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Core;

namespace CorleyEngine.Editor;

public class SceneViewWindow : EditorWindow {
    public EditorCamera Camera { get; private set; }
    public RenderTarget2D Canvas { get; private set; }

    private readonly GraphicsDevice _graphics;
    private readonly ImGuiRenderer _imGuiRenderer;

    public SceneViewWindow(GraphicsDevice graphics, ImGuiRenderer imGuiRenderer) : base("Scene View") {
        _graphics = graphics;
        _imGuiRenderer = imGuiRenderer;
        Camera = new EditorCamera();
    }

    /// <summary>
    /// Renders the current scene to the GameCanvas.
    /// Must be called from the main Draw method BEFORE ImGuiRenderer.BeforeLayout()
    /// </summary>
    public void RenderSceneToCanvas(SpriteBatch spriteBatch, Scene activeScene) {
        if (Canvas == null || activeScene == null) return;

        // 1. Hijack the GPU to draw to our specific Scene View canvas
        _graphics.SetRenderTarget(Canvas);
        _graphics.Clear(new Color(45, 45, 48)); // Dark grey to distinguish from Game View

        // 2. Tell the scene to draw using the Editor Camera's matrix
        Vector2 canvasSize = new (Canvas.Width, Canvas.Height);
        activeScene.Draw(spriteBatch, Camera.GetViewMatrix(canvasSize));

        _graphics.SetRenderTarget(null);
    }

    protected override void OnGui(GameTime gameTime) {

        System.Numerics.Vector2 size = ImGui.GetContentRegionAvail();
        if (size.X <= 0 || size.Y <= 0) return;

        if (Canvas == null || Canvas.Width != (int)size.X || Canvas.Height != (int)size.Y) {
            Canvas?.Dispose();
            Canvas = new RenderTarget2D(_graphics, (int)size.X, (int)size.Y);
        }

        if (Canvas != null) {
            ImGui.Image(_imGuiRenderer.BindTexture(Canvas), size);
        }

        HandleCameraControls();

    }

    private void HandleCameraControls() {

        // Only move the camera if the mouse is inside this specific window
        if (ImGui.IsWindowHovered()) {
            var io = ImGui.GetIO();

            // Middle mouse pan.
            if (ImGui.IsMouseDragging(ImGuiMouseButton.Middle)) {
                // ImGui gives us exact pixel deltas. We divide by zoom so it pans consistently.
                Camera.Position.X -= io.MouseDelta.X / Camera.Zoom;
                Camera.Position.Y -= io.MouseDelta.Y / Camera.Zoom;
            }

            // Zoom in and out with scroll wheel.
            if (io.MouseWheel != 0) {
                Camera.Zoom += io.MouseWheel * 0.1f;
                if (Camera.Zoom < 0.1f) Camera.Zoom = 0.1f; // Prevent inverting or zero zoom
            }
        }
    }
}