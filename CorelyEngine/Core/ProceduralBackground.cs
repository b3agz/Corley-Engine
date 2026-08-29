using Raylib_cs;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// A procedural background that renders a blue top half and a grey bottom half,
/// covering the full screen regardless of game size.
/// </summary>
public class ProceduralBackground : RenderableObject {

    /// <summary>
    /// Initializes a new instance of the <see cref="ProceduralBackground"/> class.
    /// </summary>
    public ProceduralBackground() : base() {
        Name = "Procedural Background";
        Depth = 0; // Ensures it is drawn first (lowest depth)
    }

    /// <summary>
    /// Draws the background to the screen, ensuring the horizon (world Y = 0)
    /// is at the center line of the screen, and the background fills the entire camera view.
    /// </summary>
    public override void OnDraw() {
        var cam = SceneManager.ActiveScene.Camera;

        // Set corners to just outside the bounds of the world to avoid weird lagging effect where camera moves but the rectangles
        // haven't updated yet.
        Vector2 topLeft = cam.ScreenToWorld(new (-5, -5));
        Vector2 bottomRight = cam.ScreenToWorld(new Vector2(EngineConstants.RESOLUTION_WIDTH + 5, EngineConstants.RESOLUTION_HEIGHT + 5));

        float width = bottomRight.X - topLeft.X;

        // Sky (Top: covers up to world Y = 0 to top of screen)
        float blueY = topLeft.Y;
        float blueHeight = Math.Max(0, Math.Min(bottomRight.Y, 0) - topLeft.Y);
        if (blueHeight > 0) {
            Raylib.DrawRectangle((int)topLeft.X, (int)blueY, (int)width, (int)blueHeight, Color.Blue);
        }

        // Floor (Bottom: covers from world Y = 0 to bottom of screen)
        float grayY = Math.Max(topLeft.Y, 0);
        float grayHeight = Math.Max(0, bottomRight.Y - Math.Max(topLeft.Y, 0));
        if (grayHeight > 0) {
            Raylib.DrawRectangle((int)topLeft.X, (int)grayY, (int)width, (int)grayHeight, Color.Gray);
        }
    }

    /// <summary>
    /// Checks if a point is within the bounds of the background.
    /// Since the background follows the camera, this is always true for points
    /// currently visible in the camera view.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is within the camera view.</returns>
    public override bool IsPointInside(Vector2 point) {
        var cam = SceneManager.ActiveScene.Camera;
        Vector2 topLeft = cam.ScreenToWorld(Vector2.Zero);
        Vector2 bottomRight = cam.ScreenToWorld(new Vector2(EngineConstants.RESOLUTION_WIDTH, EngineConstants.RESOLUTION_HEIGHT));

        return point.X >= topLeft.X && point.X <= bottomRight.X &&
               point.Y >= topLeft.Y && point.Y <= bottomRight.Y;
    }

    /// <summary>
    /// Gets the size of the background (the full screen resolution).
    /// </summary>
    /// <returns>The size of the background.</returns>
    public override Vector2 GetSize() {
        return new Vector2(EngineConstants.RESOLUTION_WIDTH, EngineConstants.RESOLUTION_HEIGHT);
    }
}
