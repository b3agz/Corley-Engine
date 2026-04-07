using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using CorleyEngine.Core;
using CorleyEngine.IO;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace CorleyEngine.Editor;

public class CorleyEditor : CorleyGame {

    private GraphicsDeviceManager _graphics;

    private ImGuiRenderer _imGuiRenderer;
    private StatusBar _statusBar;
    private MainWorkspace _workspace;

    private GameViewWindow _gameView;
    private SceneViewWindow _sceneView;

    private EditorPreferences _preferences;
    private readonly string _prefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "editor_prefs.json");

    List<EditorWindow> _windows = new();

    public CorleyEditor() {

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // TODO: Store previous window size and re-use it.
        _graphics.PreferredBackBufferWidth = 1440;
        _graphics.PreferredBackBufferHeight = 810;

    }

    protected override void Initialize() {

        try {

            Console.WriteLine("=== CORLEY EDITOR INITIALIZING ===");

            ImGui.CreateContext();

            LoadPreferences();

            _imGuiRenderer = new ImGuiRenderer(this);

            _preferences.ApplyTo(ImGui.GetStyle());

            string absoluteFontPath = GetFontPath(_preferences.FontPath);
            Console.WriteLine($"[Editor] Searching for font at: {absoluteFontPath}");

            if (File.Exists(absoluteFontPath)) {
                ImGui.GetIO().Fonts.AddFontFromFileTTF(absoluteFontPath, _preferences.FontSize);
                Console.WriteLine("[Editor] Font loaded successfully.");
            }
            else {
                Console.WriteLine("[Editor] WARNING: Font file not found!");
            }

            _imGuiRenderer.RebuildFontAtlas();

            // ! Temporary project load for testing.
            ProjectManager.LoadProject(@"D:\Corley Engine\CorleyEngine.Runtime\SampleProject\Sample Project.corleyproject");
            AssetManager.Initialise(@"D:\Corley Engine\CorleyEngine.Runtime\SampleProject\Assets", GraphicsDevice);

            var io = ImGui.GetIO();
            io.ConfigWindowsMoveFromTitleBarOnly = true;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            _statusBar = new StatusBar();
            _workspace = new MainWorkspace();

            // Pass dependencies through constructors
            _gameView = new GameViewWindow(this, _imGuiRenderer);
            _sceneView = new SceneViewWindow(GraphicsDevice, _imGuiRenderer);

            // Add to your list of windows to draw later
            _windows.Add(_gameView);
            _windows.Add(_sceneView);

            base.Initialize();
            Console.WriteLine("=== INITIALIZATION COMPLETE ===");
        }
        catch (Exception ex) {
            Console.WriteLine($"Critical Error: {ex.Message}");
        }

    }

    protected override void LoadContent() {

        // ! Temporary project load for testing.
        SceneManager.LoadScene("TestLevel");

        SetWindowTitle("Corley Engine Editor v0.1.0");

        InspectorWindow inspector = new();


        List<Entity> entities = SceneManager.ActiveScene?.GetEntities();

        inspector.TargetEntity = entities[0];

        _windows.Add(inspector);


        base.LoadContent();

    }

    protected override void Update(GameTime gameTime) {

        // TODO: By default, Time will operate from when the editor launched. It should run (and reset) when play mode is initiated.
        Time.Update(gameTime);

        // We want to get the game input regardless. Game input is tied to the game view, and we'll need
        // relative cursor positions to be able to move objects.
        Input.Update();

        // TODO: If PlayMode, call update function on the active scene.
        SceneManager.ActiveScene?.Update();

        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime) {

        _imGuiRenderer.BeforeLayout(gameTime);

        _workspace.Draw();

        // Top bar/drop down menus.
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

        _statusBar.RightMessage = $"FPS: {1000f / gameTime.ElapsedGameTime.TotalMilliseconds:0} | Corley Engine";
        _statusBar.Draw();

        // Draw the Game View window.
        ImGui.Begin("Game View", ImGuiWindowFlags.NoScrollbar);

        // Set the size of the Game View and resize the canvas if necessary.
        System.Numerics.Vector2 size = ImGui.GetContentRegionAvail();
        int newWidth = (int)size.X;
        int newHeight = (int)size.Y;
        BuildCanvas(newWidth, newHeight);

        // Update Input so it registers the mouse position correctly.
        System.Numerics.Vector2 contentPos = ImGui.GetCursorScreenPos();
        System.Numerics.Vector2 contentSize = ImGui.GetContentRegionAvail();

        Input.ViewportOffset = new Vector2(contentPos.X, contentPos.Y);
        Input.ViewportDisplaySize = new Vector2(contentSize.X, contentSize.Y);
        Input.ViewportScale = new Vector2((float)RenderWidth / contentSize.X, (float)RenderHeight / contentSize.Y);

        // Draw the game logic here. If we do it up top, the game view flickers when the
        // camera moves. If we do it at the end, we get a black screen with no UI.
        base.Draw(gameTime);

        ImGui.Image(_imGuiRenderer.BindTexture(GameCanvas), size);
        ImGui.End();

        // Render the scene to the sceneview window. Needs to be done
        _sceneView.RenderSceneToCanvas(_sceneBatch, SceneManager.ActiveScene);

        // Loop through any open EditorWindows and draw them.
        foreach (EditorWindow window in _windows) {
            window.Draw(gameTime);
        }

        //DrawSceneView();

        _imGuiRenderer.AfterLayout();

    }

    private void LoadPreferences() {

        string prefPath = GetPrefsPath();

        if (File.Exists(prefPath)) {
            Console.WriteLine($"[Editor] Found prefs at: {prefPath}");
            _preferences = DataSerializer.Load<EditorPreferences>(prefPath);
        }
        else {
            Console.WriteLine($"[Editor] No prefs found. Generating default editor_prefs.json and saving to {prefPath}...");
            _preferences = new EditorPreferences();
            DataSerializer.Save(_preferences, prefPath);
        }

    }

    private string GetRootFolder([CallerFilePath] string sourceFilePath = "") {
        string? dir = Path.GetDirectoryName(sourceFilePath);
        return Directory.Exists(dir) ? dir : AppDomain.CurrentDomain.BaseDirectory;
    }

    private string GetPrefsPath() {
        return Path.Combine(GetRootFolder(), "editor_prefs.json");
    }

    private string GetFontPath(string relativePath) {
        return Path.Combine(GetRootFolder(), relativePath);
    }

}