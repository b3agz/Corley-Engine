using Raylib_cs;

namespace CorleyEngine.Core;

/// <summary>
/// A CorelyObject for actors. These are essentially NPCs, they can be interacted with and have dialogue.
/// </summary>
public class Actor() : CorleyObject(true) {

    public override void OnDraw() {
        Raylib.DrawRectangle((int)Position.X, (int)Position.Y, 40, 80, Color.Maroon);
    }

}