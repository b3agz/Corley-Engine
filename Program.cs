using System;
using System.IO;
using System.Numerics;
using Raylib_cs;
using CorleyEngine.Core;

namespace CorleyEngine;

// ========================================================================
// MAIN ENGINE LOOP
// ========================================================================
class Program {

    // The size of the game window when loaded.
    // TODO: Engine should store the state of the window the last time the game was run and use that when loaded.
    const int TargetWidth = 1280;
    const int TargetHeight = 720;

    static void Main() {

        // Initialisation stuff.
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow | ConfigFlags.VSyncHint);
        Raylib.SetTargetFPS(60);
        Raylib.InitWindow(TargetWidth, TargetHeight, "Corley Engine");
        Raylib.SetWindowMinSize(TargetWidth, TargetHeight);

        // Maximize it immediately so it fills the screen without breaking bounds
        //Raylib.MaximizeWindow();

        // Create a texture to draw the game to. This allows us to keep the game resolution consistent regardless of the window size.
        RenderTexture2D target = Raylib.LoadRenderTexture(EngineConstants.RESOLUTION_WIDTH, EngineConstants.RESOLUTION_HEIGHT);
        Raylib.SetTextureFilter(target.Texture, TextureFilter.Point);

        // Set up the game camera.
        Camera2D camera = new() {
            Target = Vector2.Zero,
            Offset = Vector2.Zero,
            Rotation = 0.0f,
            Zoom = 1.0f
        };

        Scene _activeScene = new Scene("Test Scene", camera);
        _activeScene.Init();

        // Initialize Cursor
        string cursorPath = "./Sprites/default_cursor.png";
        CorelyCursor cursor;
        if (File.Exists(cursorPath)) {
            cursor = new CorelyCursor(cursorPath, new Vector2(2, 2));
        }
        else {
            Image tempImg = Raylib.GenImageColor(16, 16, Color.Magenta);
            Texture2D tempTex = Raylib.LoadTextureFromImage(tempImg);
            Raylib.UnloadImage(tempImg);
            cursor = new CorelyCursor(tempTex, new Vector2(0, 0));
        }

        // Hide the default OS cursor so it doesn't float over our custom one
        Raylib.HideCursor();

        // The core engine loop. Everything that happens during the game originates in this while loop.
        while (!Raylib.WindowShouldClose()) {

            float deltaTime = Raylib.GetFrameTime();

            // Trigger all active update objects.
            EngineEvents.TickUpdate(deltaTime);
            cursor.Update(deltaTime);

            // Find the maximum scale that fits the window based on the INTERNAL game resolution
            float scale = Math.Min(
                (float)Raylib.GetScreenWidth() / EngineConstants.RESOLUTION_WIDTH,
                (float)Raylib.GetScreenHeight() / EngineConstants.RESOLUTION_HEIGHT
            );

            // Calculate the letterbox/pillarbox offsets against the internal resolution. This is what keeps the game resolution
            // consistent regardless of what the size of the window is.
            float offsetX = (Raylib.GetScreenWidth() - (EngineConstants.RESOLUTION_WIDTH * scale)) * 0.5f;
            float offsetY = (Raylib.GetScreenHeight() - (EngineConstants.RESOLUTION_HEIGHT * scale)) * 0.5f;

            // Map the system mouse cursor's position to its position in the game window.
            Vector2 rawMouse = Raylib.GetMousePosition();
            float virtualX = (rawMouse.X - offsetX) / scale;
            float virtualY = (rawMouse.Y - offsetY) / scale;

            // Clamped for in-game interaction logic
            Vector2 virtualMouse = new(
                (int)Math.Clamp(Math.Round(virtualX), 0, EngineConstants.RESOLUTION_WIDTH),
                (int)Math.Clamp(Math.Round(virtualY), 0, EngineConstants.RESOLUTION_HEIGHT)
            );

            // Draw everything to the render texture.
            Raylib.BeginTextureMode(target);
            Raylib.ClearBackground(Color.Black);

            Raylib.BeginMode2D(camera);

            // Trigger all active render objects.
            EngineEvents.TickDraw();

            Raylib.EndMode2D();

            // TEST STUFF
            // In the fullness of time, we won't be drawing anything directly in here, this part of the code is just for showing something at this stage of dev.
            Raylib.DrawText("RENDER TEXTURE UI LAYER", 20, 20, 20, Color.RayWhite);
            Raylib.DrawText(
                $"Virtual Mouse: {virtualMouse.X}x{virtualMouse.Y}",
                (int)virtualMouse.X + 20,
                (int)virtualMouse.Y + 20,
                20,
                Color.Green
            );

            // Draw the cursor unclamped so it looks natural if the user moves the cursor out of the game area.
            Vector2 virtualMouseUnclamped = new (virtualX, virtualY);
            cursor.Draw(virtualMouseUnclamped, 1.0f);

            Raylib.EndTextureMode();

            // Draw the render texture to the game window.
            Raylib.BeginDrawing();

            // This is the colour that will be shown outside of the texture (the pillar boxing/letter boxing).
            Raylib.ClearBackground(Color.Black);

            // OpenGL render textures are vertically flipped. Passing a negative height in the source rectangle fixes this.
            Rectangle sourceRec = new (0.0f, 0.0f, (float)target.Texture.Width, (float)-target.Texture.Height);

            // Define where and how big the texture should be drawn on the physical screen
            Rectangle destRec = new (offsetX, offsetY, EngineConstants.RESOLUTION_WIDTH * scale, EngineConstants.RESOLUTION_HEIGHT * scale);

            Raylib.DrawTexturePro(target.Texture, sourceRec, destRec, Vector2.Zero, 0.0f, Color.White);

            Raylib.EndDrawing();
        }

        // Handle shutdown.
        cursor.Unload();
        Raylib.UnloadRenderTexture(target);
        Raylib.CloseWindow();
    }
}