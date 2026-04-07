using Microsoft.Xna.Framework;

namespace CorleyEngine.Core;

/// <summary>
/// A static class that provides access to information related to the game view.
/// </summary>
/// <remarks>
/// During runtime, the game view is essentially just the game window. In the
/// editor, it is the game view window.
/// </remarks>
public static class GameView {

    public static int Width { get; private set; }
    public static int Height { get; private set; }

    // The resolution you are building the game for (e.g., 1080p)
    public const int DesignWidth = 1920;
    public const int DesignHeight = 1080;

    public static Vector2 Center => new Vector2(Width * 0.5f, Height * 0.5f);

    // This calculates how much to shrink/grow the camera view
    public static float ScaleFactor => (float)Height / DesignHeight;

    internal static void UpdateDimensions(int width, int height) {
        Width = width;
        Height = height;
    }

}