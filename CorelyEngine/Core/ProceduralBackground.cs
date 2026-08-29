using Raylib_cs;
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
}
