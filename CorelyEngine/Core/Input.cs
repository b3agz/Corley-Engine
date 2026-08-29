using Raylib_cs;
using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// Provides a central wrapper for Raylib input functions, ensuring all input handling
/// is routed through the engine.
/// </summary>
public static class Input {

    /// <summary>
    /// Checks if a mouse button is pressed once this frame.
    /// </summary>
    /// <param name="button">The mouse button to check.</param>
    /// <returns>True if the button was pressed.</returns>
    public static bool IsMouseButtonPressed(MouseButton button) => Raylib.IsMouseButtonPressed(button);

    /// <summary>
    /// Checks if a mouse button is currently held down.
    /// </summary>
    /// <param name="button">The mouse button to check.</param>
    /// <returns>True if the button is held down.</returns>
    public static bool IsMouseButtonDown(MouseButton button) => Raylib.IsMouseButtonDown(button);

    /// <summary>
    /// Checks if a mouse button was released this frame.
    /// </summary>
    /// <param name="button">The mouse button to check.</param>
    /// <returns>True if the button was released.</returns>
    public static bool IsMouseButtonReleased(MouseButton button) => Raylib.IsMouseButtonReleased(button);

    /// <summary>
    /// Checks if a key is pressed once this frame.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key was pressed.</returns>
    public static bool IsKeyPressed(KeyboardKey key) => Raylib.IsKeyPressed(key);

    /// <summary>
    /// Checks if a key is currently held down.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key is held down.</returns>
    public static bool IsKeyDown(KeyboardKey key) => Raylib.IsKeyDown(key);

    /// <summary>
    /// Checks if a key was released this frame.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key was released.</returns>
    public static bool IsKeyReleased(KeyboardKey key) => Raylib.IsKeyReleased(key);

    /// <summary>
    /// Gets the current mouse position on the screen.
    /// </summary>
    /// <returns>The mouse position as a Vector2.</returns>
    public static Vector2 GetMousePosition() => Raylib.GetMousePosition();

    /// <summary>
    /// Gets the normalized mouse position (0.0 to 1.0) on the screen.
    /// </summary>
    /// <returns>The normalized mouse position as a Vector2.</returns>
    public static Vector2 GetMousePositionNormalized() {
        Vector2 pos = Raylib.GetMousePosition();
        return new Vector2(pos.X / Raylib.GetScreenWidth(), pos.Y / Raylib.GetScreenHeight());
    }

    /// <summary>
    /// Gets the mouse position in game-space (virtual resolution), unclamped.
    /// </summary>
    /// <returns>The mouse position in game-space as a Vector2.</returns>
    public static Vector2 GetMousePositionGameSpaceUnclamped() {
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
    /// Gets the mouse position in game-space (virtual resolution), clamped to the game area.
    /// </summary>
    /// <returns>The mouse position in game-space as a Vector2.</returns>
    public static Vector2 GetMousePositionGameSpace() {
        Vector2 pos = GetMousePositionGameSpaceUnclamped();
        return new Vector2(
            (float)Math.Clamp(Math.Round(pos.X), 0, EngineConstants.RESOLUTION_WIDTH),
            (float)Math.Clamp(Math.Round(pos.Y), 0, EngineConstants.RESOLUTION_HEIGHT)
        );
    }
}
