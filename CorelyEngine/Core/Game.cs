using System;
using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// Abstract base class for all games built with the Corley Engine.
/// Handles the engine lifecycle, including initialisation, the main loop, and shutdown.
/// </summary>
public abstract class Game {

    protected Camera _camera;
    protected RenderTexture2D _target;
    protected CorelyCursor _cursor;

    protected const int TargetWidth = 1280;
    protected const int TargetHeight = 720;

    /// <summary>
    /// Called when the game is initialised. Use this to set up scenes and game-specific data.
    /// </summary>
    protected abstract void OnInitialize();

    /// <summary>
    /// Called when the game should load content.
    /// </summary>
    protected abstract void OnLoadContent();

    /// <summary>
    /// Called once per frame. Use this to update game logic.
    /// </summary>
    /// <param name="deltaTime">The amount of time in seconds that has elapsed since the last frame.</param>
    protected abstract void OnUpdate(float deltaTime);

    /// <summary>
    /// Runs the game loop.
    /// </summary>
    public void Run() {
        CorleyLog.LogInfo($"Initialising Corley Engine...");

        Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.VSyncHint);
        Raylib.SetTargetFPS(60);
        Raylib.InitWindow(TargetWidth, TargetHeight, "Corley Engine");
        Raylib.SetWindowMinSize(TargetWidth, TargetHeight);

        _target = Raylib.LoadRenderTexture(EngineConstants.RESOLUTION_WIDTH, EngineConstants.RESOLUTION_HEIGHT);
        Raylib.SetTextureFilter(_target.Texture, TextureFilter.Point);

        _camera = new Camera();

        _cursor = new();
        Raylib.HideCursor();

        OnInitialize();
        OnLoadContent();

        CorleyLog.LogInfo("Initialisation complete, starting game loop.");

        while (!Raylib.WindowShouldClose()) {
            float deltaTime = Raylib.GetFrameTime();

            // Core engine updates
            EngineEvents.TickUpdate(deltaTime);
            _cursor.Update(deltaTime);

            // Game specific updates
            OnUpdate(deltaTime);

            // Calculations
            float scale = Math.Min(
                (float)Raylib.GetScreenWidth() / EngineConstants.RESOLUTION_WIDTH,
                (float)Raylib.GetScreenHeight() / EngineConstants.RESOLUTION_HEIGHT
            );

            float offsetX = (Raylib.GetScreenWidth() - (EngineConstants.RESOLUTION_WIDTH * scale)) * 0.5f;
            float offsetY = (Raylib.GetScreenHeight() - (EngineConstants.RESOLUTION_HEIGHT * scale)) * 0.5f;

            // Mapping mouse
            Vector2 virtualMouse = Input.GetVirtualMousePosition();
            Vector2 worldMouse = _camera.ScreenToWorld(virtualMouse);

            // Draw to texture
            Raylib.BeginTextureMode(_target);
            Raylib.ClearBackground(SceneManager.ActiveScene.BackgroundColour);
            Raylib.BeginMode2D(_camera.GetRaylibCamera());
            
            RenderManager.DrawAll();
            
            Raylib.EndMode2D();
            
            _cursor.Draw(Input.GetVirtualMousePositionUnclamped(), 1.0f);
            Raylib.EndTextureMode();

            // Draw to window
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Rectangle sourceRec = new(0.0f, 0.0f, (float)_target.Texture.Width, (float)-_target.Texture.Height);
            Rectangle destRec = new(offsetX, offsetY, EngineConstants.RESOLUTION_WIDTH * scale, EngineConstants.RESOLUTION_HEIGHT * scale);

            Raylib.DrawTexturePro(_target.Texture, sourceRec, destRec, Vector2.Zero, 0.0f, Color.White);
            Raylib.EndDrawing();
        }

        Shutdown();
    }

    protected virtual void Shutdown() {
        CorleyLog.LogInfo("Shutting down Corley Engine...");
        _cursor.Unload();
        Assets.UnloadAll();
        Raylib.UnloadRenderTexture(_target);
        Raylib.CloseWindow();
    }
}
