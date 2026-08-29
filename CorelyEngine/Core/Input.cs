using Raylib_cs;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// Provides a central wrapper for Raylib input functions, ensuring all input handling 
/// is routed through the engine's central Input Manager.
/// </summary>
public static class Input {

    /// <summary>
    /// Checks if a mouse button is pressed once this frame.
    /// </summary>
    public static bool IsMouseButtonPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);

    /// <summary>
    /// Checks if a mouse button is currently held down.
    /// </summary>
    public static bool IsMouseButtonDown(MouseButton button) => Raylib.IsMouseButtonDown(button);

    /// <summary>
    /// Checks if a mouse button was released this frame.
    /// </summary>
    public static bool IsMouseButtonReleased(MouseButton button) => Raylib.IsMouseButtonReleased(button);

    /// <summary>
    /// Checks if a key is pressed once this frame.
    /// </summary>
    public static bool IsKeyPressed(KeyboardKey key) => Raylib.IsKeyPressed(key);

    /// <summary>
    /// Checks if a key is currently held down.
    /// </summary>
    public static bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);

    /// <summary>
    /// Checks if a key was released this frame.
    /// </summary>
    public static bool IsKeyReleased(KeyboardKey key) => Raylib.IsKeyReleased(key);

    /// <summary>
    /// Gets the current mouse position in the physical window.
    /// </summary>
    public static Vector2 GetRawMousePosition() => Raylib.GetMousePosition();

    /// <summary>
    /// Gets the normalized mouse position (0.0 to 1.0) on the physical screen.
    /// </summary>
    public static Vector2 GetNormalizedMousePosition() {
        Vector2 pos = Raylib.GetMousePosition();
        return new Vector2(pos.X / Raylib.GetScreenWidth(), pos.Y / Raylib.GetScreenHeight());
    }

    /// <summary>
    /// Gets the mouse position in the game's virtual resolution space, unclamped.
    /// </summary>
    public static Vector2 GetVirtualMousePositionUnclamped() {
        float scale = Math.Min(
            (float)Raylib.GetScreenWidth() / EngineConstants.RESOLUTION_WIDTH,
            (float)Raylib.GetScreenHeight() / EngineConstants.RESOLUTION_HEIGHT
        );

        float offsetX = (Raylib.GetScreenWidth() - (EngineConstants.RESOLUTION_WIDTH * scale)) * 0.5f;
        float offsetY = (Raylib.GetScreenHeight() - (EngineConstants.RESOLUTION_HEIGHT * scale)) * 0.5f;

        Vector2 rawMouse = Raylib.GetMousePosition();
        return new Vector2((rawMouse.X - offsetX) / scale, (rawMouse.Y - offsetY) / scale);
    }

    /// <summary>
    /// Gets the mouse position in the game's virtual resolution space, clamped to the game area.
    /// </summary>
    public static Vector2 GetVirtualMousePosition() {
        Vector2 pos = GetVirtualMousePositionUnclamped();
        return new Vector2(
            (float)Math.Clamp(Math.Round(pos.X), 0, EngineConstants.RESOLUTION_WIDTH),
            (float)Math.Clamp(Math.Round(pos.Y), 0, EngineConstants.RESOLUTION_HEIGHT)
        );
    }
}
