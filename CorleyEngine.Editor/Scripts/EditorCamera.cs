using Microsoft.Xna.Framework;

namespace CorleyEngine.Editor;

public class EditorCamera {
    public Vector2 Position = Vector2.Zero;
    public float Zoom = 1f;

    // We pass in the current size of the Scene View window so it stays centered
    public Matrix GetViewMatrix(Vector2 viewportSize) {
        Vector2 center = new Vector2(viewportSize.X * 0.5f, viewportSize.Y * 0.5f);

        Matrix positionOffset = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0));
        Matrix scaleOffset = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f));
        Matrix centerOffset = Matrix.CreateTranslation(new Vector3(center.X, center.Y, 0));

        return positionOffset * scaleOffset * centerOffset;
    }

    public Vector2 ScreenToWorldSpace(Vector2 screenPosition, Vector2 viewportSize) {
        Matrix inverse = Matrix.Invert(GetViewMatrix(viewportSize));
        return Vector2.Transform(screenPosition, inverse);
    }
}