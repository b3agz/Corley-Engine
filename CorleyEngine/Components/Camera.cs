using Microsoft.Xna.Framework;
using CorleyEngine.Core;

namespace CorleyEngine.Components;

public class Camera : Component {

    public static Camera MainCamera;

    public float Zoom = 1f;

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

    /// <summary>
    /// Calculates the view matrix for this camera.
    /// </summary>
    /// <returns>The view matrix.</returns>
    public Matrix GetViewMatrix() {

        Vector2 center = GameView.Center;

        // Combine the engine's scale factor with the camera's zoom factor.
        float finalZoom = (Zoom == 0f ? 1f : Zoom) * GameView.ScaleFactor;

        // Centre on the camera transform.
        Matrix positionOffset = Matrix.CreateTranslation(
            new Vector3(-Transform.Position.X, -Transform.Position.Y, 0));

        // Scale the game world according to the finalZoom value.
        Matrix scaleOffset = Matrix.CreateScale(
            new Vector3(finalZoom, finalZoom, 1f));

        // Move everything so that the centre of the view is still centred after all the
        // modifications.
        Matrix centerOffset = Matrix.CreateTranslation(
            new Vector3(center.X, center.Y, 0));

        return positionOffset * scaleOffset * centerOffset;

    }

}