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

    public static void Update() {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();
    }

    /// <summary>
    /// The position of the cursor (regardless of what is controlling it).
    /// </summary>
    public static Vector2 CursorPosition => new(_currentMouse.X, _currentMouse.Y);

    /// <summary>
    /// Checks if the action input was triggered this frame.
    /// </summary>
    /// <param name="actionName">The name of the action being checked.</param>
    /// <returns>True if the action was triggered this frame.</returns>
    public static bool IsActionTriggered(string actionName) {
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
        return actionName switch {
            "Primary" => _currentMouse.LeftButton == ButtonState.Pressed,
            "Secondary" => _currentMouse.RightButton == ButtonState.Pressed,
            _ => false
        };
    }
}