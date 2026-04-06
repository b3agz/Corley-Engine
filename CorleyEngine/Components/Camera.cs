using Microsoft.Xna.Framework;
using CorleyEngine.Core;

namespace CorleyEngine.Components;

public class Camera : Component {

    public static Camera MainCamera;

    // You'll eventually want to read this from your Engine setup,
    // but we'll hardcode a standard 800x600 window for the skeleton.
    private readonly Vector2 _screenCenter = new Vector2(400, 300);

    public override void Awake() {
        MainCamera = this;
        // TODO: Some kind of camera management system to handle multiple cameras.
        // Right now, the most recently added camera becomes the main camera regardless.
    }

    /// <summary>
    /// Converts a position on the screen into the corresponding position in the game
    /// world after factoring in the Camera position.
    /// </summary>
    /// <param name="screenPosition">The position on the screen.</param>
    /// <returns>A Vector2 representing the position in world space.</returns>
    public Vector2 ScreenToWorldSpace(Vector2 screenPosition) {

        Matrix inverseViewMatrix = Matrix.Invert(GetViewMatrix());
        return Vector2.Transform(screenPosition, inverseViewMatrix);

    }

    public Matrix GetViewMatrix() {
        if (Transform == null) return Matrix.Identity;

        // Step 1: Move the universe in the OPPOSITE direction of the camera
        Matrix positionOffset = Matrix.CreateTranslation(
            new Vector3(-Transform.Position.X, -Transform.Position.Y, 0));

        // Step 2: Push everything so the camera's coordinates are in the dead center of the screen,
        // rather than the top-left corner.
        Matrix centerOffset = Matrix.CreateTranslation(
            new Vector3(_screenCenter.X, _screenCenter.Y, 0));

        // Step 3: Combine them (Order matters in matrix math!)
        return positionOffset * centerOffset;

    }
}
