using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using CorleyEngine.Core;

namespace CorleyEngine.Editor;

public class CorleyEditor : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // The object that draws UI over your game window
    private ImGuiRenderer _imGuiRenderer;

    public CorleyEditor() {

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // TODO: Store previous window size and re-use it.
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

    }

    protected override void Initialize() {

        ImGui.CreateContext();

        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();

        base.Initialize();

    }

    protected override void LoadContent() {

        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime) {

        // Unlike the runtime class, we don't call Update on any entities in the editor.
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        // If we have an active scene, draw it.
        SceneManager.ActiveScene?.Draw(_spriteBatch);

        _imGuiRenderer.BeforeLayout(gameTime);

        // Placeholder/Test UI Stuff
        if (ImGui.BeginMainMenuBar()) {
            if (ImGui.BeginMenu("File")) {
                if (ImGui.MenuItem("Save Scene", "Ctrl+S")) { /* Save Logic */ }
                if (ImGui.MenuItem("Exit Editor")) { Exit(); }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("View")) {
                ImGui.MenuItem("Entity Inspector");
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        // Floating project status widow.
        ImGui.Begin("Engine Status");
        ImGui.Text("Corley Editor is online.");
        ImGui.Separator();

        // FPS display
        ImGui.Text($"FPS: {1000f / gameTime.ElapsedGameTime.TotalMilliseconds:0.0}");
        ImGui.Text($"Uptime: {gameTime.TotalGameTime.TotalSeconds:0.0}s");

        ImGui.Spacing();
        ImGui.TextColored(new System.Numerics.Vector4(1, 0.8f, 0.2f, 1), "No Project Loaded");

        ImGui.End();

        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);

    }
}