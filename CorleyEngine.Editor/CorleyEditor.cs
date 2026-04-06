using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ImGuiNET;
using CorleyEngine.Core;
using CorleyEngine.IO;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace CorleyEngine.Editor;

public class CorleyEditor : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private ImGuiRenderer _imGuiRenderer;

    private EditorPreferences _preferences;
    private readonly string _prefsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "editor_prefs.json");

    public CorleyEditor() {

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // TODO: Store previous window size and re-use it.
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;

    }

    protected override void Initialize() {

        try {

            Console.WriteLine("=== CORLEY EDITOR INITIALIZING ===");

            ImGui.CreateContext();

            LoadPreferences();

            _imGuiRenderer = new ImGuiRenderer(this);

            ApplyStyle();

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

            base.Initialize();
            Console.WriteLine("=== INITIALIZATION COMPLETE ===");
        }
        catch (Exception ex) {
            Console.WriteLine($"Critical Error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

    }

    protected override void LoadContent() {

        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime) {

        // Unlike the runtime class, we don't call Update on any entities in the editor.
        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime) {

        GraphicsDevice.Clear(Color.CornflowerBlue);

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

        ImGui.Begin("Style Editor");
        ImGui.ShowStyleEditor(); // This is the gold mine
        ImGui.End();

        _imGuiRenderer.AfterLayout();

        base.Draw(gameTime);

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

    private void ApplyStyle() {

        var style = ImGui.GetStyle();

        // Apply Geometry
        style.WindowRounding = _preferences.WindowRounding;
        style.FrameRounding = _preferences.FrameRounding;
        style.GrabRounding = _preferences.GrabRounding;
        style.WindowBorderSize = _preferences.WindowBorderSize;
        style.ScrollbarSize = _preferences.ScrollbarSize;
        style.FramePadding = _preferences.FramePadding;
        style.WindowTitleAlign = _preferences.WindowTitleAlign;
        style.WindowPadding = _preferences.WindowPadding;

        // Apply Colors
        foreach (var colorKvp in _preferences.ThemeColors) {
            // Convert the string key (e.g. "WindowBg") back into the ImGuiCol enum
            if (Enum.TryParse<ImGuiCol>(colorKvp.Key, out var imguiColorTarget)) {
                style.Colors[(int)imguiColorTarget] = colorKvp.Value;
            }
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