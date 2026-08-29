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
    /// Draws the background to the screen.
    /// </summary>
    public override void OnDraw() {
        int width = EngineConstants.RESOLUTION_WIDTH;
        int height = EngineConstants.RESOLUTION_HEIGHT;

        // Draw top half blue
        Raylib.DrawRectangle(0, 0, width, height / 2, Color.Blue);

        // Draw bottom half grey
        Raylib.DrawRectangle(0, height / 2, width, height / 2, Color.Gray);
    }

    /// <summary>
    /// Checks if a point is within the bounds of the background.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <returns>True if the point is within the bounds, false otherwise.</returns>
    public override bool IsPointInside(Vector2 point) {
        return point.X >= 0 && point.X <= EngineConstants.RESOLUTION_WIDTH &&
               point.Y >= 0 && point.Y <= EngineConstants.RESOLUTION_HEIGHT;
    }
}
