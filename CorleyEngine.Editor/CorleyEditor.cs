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
    private TitleBar _titleBar;
    private MenuBar _menuBar;

    private GameViewWindow _gameView;
    private SceneViewWindow _sceneView;
    private ConsoleWindow _console;
    private InspectorWindow _inspector;

    public static ImFontPtr StatusBarFont { get; private set; }

    public static EditorPreferences Preferences { get; private set; }

    public EditorState State { get; private set; }

    List<EditorWindow> _windows = [];

    public CorleyEditor() {

        Log.Info("[CorleyEditor] Starting Editor...");

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Preferences = EditorPreferences.LoadOrCreate(GetRootFolder());

        Window.IsBorderless = true;
        Window.AllowUserResizing = false;

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;

    }

    protected override void Initialize() {

        // Initialise the engine asset manager. Program.cs has already verified we have a valid
        // Corley Engine Project and loaded it into memory before we get here. If we don't, the
        // editor never gets spun up.
        AssetManager.Initialise(ProjectManager.CurrentProject.AbsoluteAssetPath, GraphicsDevice);

        #region ImGui Initialisation

        ImGui.CreateContext();
        ImGuiIOPtr io = ImGui.GetIO();
        _imGuiRenderer = new ImGuiRenderer(this);

        Preferences.ApplyTo(ImGui.GetStyle());

        string absoluteFontPath = GetFontPath(Preferences.FontPath);


        if (File.Exists(absoluteFontPath)) {

            ImGui.GetIO().Fonts.AddFontFromFileTTF(absoluteFontPath, Preferences.FontSize);
            StatusBarFont = ImGui.GetIO().Fonts.AddFontFromFileTTF(absoluteFontPath, MathF.Round(Preferences.FontSize * 0.9f));
            Log.Info($"[CorleyEditor] Font loaded successfully.");

        } else {

            Log.Warning($"[CorleyEditor] WARNING: Font file not found at {absoluteFontPath} - falling back to default font.");

        }

        _imGuiRenderer.RebuildFontAtlas();

        io.ConfigWindowsMoveFromTitleBarOnly = true;
        io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

        #endregion

        _statusBar = new StatusBar();
        _workspace = new MainWorkspace();
        _titleBar = new TitleBar(this, _graphics, _imGuiRenderer);
        _menuBar = new(this);
        _inspector = new InspectorWindow();
        _console = new();

        // Game and scene view have dependencies in order to draw the game into the editor.
        _gameView = new GameViewWindow(this, _imGuiRenderer);
        _sceneView = new SceneViewWindow(GraphicsDevice, _imGuiRenderer);

        // Add windows to the editor. Any window in _windows is drawn.
        //_windows.Add(_gameView);
        //_windows.Add(_sceneView);
        //_windows.Add(new ConsoleWindow());
        _windows.Add(_inspector);
        _windows.Add(new HierarchyWindow(_inspector));

        InactiveSleepTime = new TimeSpan(0);

        base.Initialize();

    }

    protected override void LoadContent() {

        base.LoadContent();

        SetWindowTitle("Corley Engine Editor v0.1.0");

        // Temporary code for testing hierarchy view and inspector.
        // List<Entity> entities = SceneManager.ActiveScene?.GetEntities();
        // _inspector.TargetEntity = entities[1];
        // Entity entity = Entity.CreateStageEntity("Child", new(240, 100));
        // entity.SetParent(entities[0]);

    }

    protected override void Update(GameTime gameTime) {

        base.Update(gameTime);

        // If we're in playmode, trigger run the scene's update loop.
        if (State == EditorState.Play)
            SceneManager.ActiveScene?.Update();

    }

    protected override void Draw(GameTime gameTime) {

        _sceneView.RenderSceneToCanvas(_sceneBatch, SceneManager.ActiveScene);
        base.Draw(gameTime);
    GraphicsDevice.Clear(new Color(30, 30, 30));

        _imGuiRenderer.BeforeLayout(gameTime);

        string activeColourLabel = this.IsActive ? "TitleBg" : "MenuBarBg";
        System.Numerics.Vector4 activeColour = Preferences.ThemeColors[activeColourLabel];

        _titleBar.Draw(activeColour);

        _menuBar.Draw();

        _workspace.Draw(gameTime, _sceneView, _gameView, _console);

        _statusBar.Draw(activeColour, gameTime);


        // Loop through any open EditorWindows and draw them.
        foreach (EditorWindow window in _windows) {
            window.Draw(gameTime);
        }

        // Draw window border.
        ImGui.GetForegroundDrawList().AddRect(
            new System.Numerics.Vector2(0, 0),
            new System.Numerics.Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height),
            ImGui.GetColorU32(activeColour),
            0.0f,
            ImDrawFlags.None,
            2.0f  // Border thickness.
        );

        _imGuiRenderer.AfterLayout();

    }

    private string GetRootFolder([CallerFilePath] string sourceFilePath = "") {
        string dir = Path.GetDirectoryName(sourceFilePath);
        return Directory.Exists(dir) ? dir : AppDomain.CurrentDomain.BaseDirectory;
    }

    private string GetFontPath(string relativePath) {
        return Path.Combine(GetRootFolder(), relativePath);
    }

    private ImGuiStyleWindow _styleWindow;
    public void ToggleImGuiStyleWindow() {

        if (_styleWindow == null)
            _styleWindow = new();

        if (_windows.Contains(_styleWindow))
            _windows.Remove(_styleWindow);
        else
            _windows.Add(_styleWindow);

    }

}