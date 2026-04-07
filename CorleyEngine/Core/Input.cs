using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CorleyEngine.Core;

/// <summary>
/// Input class acts as a layer between the game and the hardware, exposing
/// actions rather than keypresses.
/// </summary>
/// <remarks>
/// Currently only supports mouse input. The intention is to support gamepad
/// and touchscreen as well.
/// </remarks>
public static class Input {

    // TODO: Create a modifiable input map.

    // TODO: Make subscribable events for input actions?

    private static MouseState _currentMouse;
    private static MouseState _previousMouse;

    /// <summary>
    /// The offset of the game viewport relative to the top-left of the window.
    /// In the game, this will be (0,0). In the editor, it will be the top corner of the Game View window .
    /// </summary>
    public static Vector2 ViewportOffset { get; set; } = Vector2.Zero;

    /// <summary>
    /// The scale ratio between the Window size and the internal GameCanvas size.
    /// </summary>
    public static Vector2 ViewportScale { get; set; } = Vector2.One;

    /// <summary>
    /// The actual width/height the viewport occupies on the screen.
    /// </summary>
    public static Vector2 ViewportDisplaySize { get; set; } = Vector2.One;

    /// <summary>
    /// Returns true if the mouse position is currently inside the game viewport. For runtime,
    /// this is the game window. For the editor, this is the game view window.
    /// </summary>
    private static bool IsMouseInViewport {

        get {
            return _currentMouse.X >= ViewportOffset.X &&
                   _currentMouse.X <= ViewportOffset.X + ViewportDisplaySize.X &&
                   _currentMouse.Y >= ViewportOffset.Y &&
                   _currentMouse.Y <= ViewportOffset.Y + ViewportDisplaySize.Y;
        }
    }

    public static void Update() {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
    }

    /// <summary>
    /// Returns the mouse position translated into Game Space coordinates.
    /// </summary>
    public static Vector2 CursorPosition {

        get {

            // Get the raw position of the mouse.
            Vector2 rawPos = new(_currentMouse.X, _currentMouse.Y);

            // Subtract the offset (where the game view starts) and
            // multiply by scale (to map window pixels to canvas pixels)
            return (rawPos - ViewportOffset) * ViewportScale;
        }
    }

    /// <summary>
    /// Checks if the action input was triggered this frame.
    /// </summary>
    /// <param name="actionName">The name of the action being checked.</param>
    /// <returns>True if the action was triggered this frame.</returns>
    public static bool IsActionTriggered(string actionName) {

        if (!IsMouseInViewport) return false;

        return actionName switch {
            "Primary" => _currentMouse.LeftButton == ButtonState.Pressed &&
                         _previousMouse.LeftButton == ButtonState.Released,
            "Secondary" => _currentMouse.RightButton == ButtonState.Pressed &&
                           _previousMouse.RightButton == ButtonState.Released,
            _ => false // Unrecognized action
        };
    }

    /// <summary>
    /// Checks if the action input is currently being held down.
    /// </summary>
    /// <param name="actionName">The name of the action being checked.</param>
    /// <returns>Returns true if the action is down this frame.</returns>
    public static bool IsActionHeld(string actionName) {

        if (!IsMouseInViewport) return false;

        return actionName switch {
            "Primary" => _currentMouse.LeftButton == ButtonState.Pressed,
            "Secondary" => _currentMouse.RightButton == ButtonState.Pressed,
            _ => false
        };
    }
}