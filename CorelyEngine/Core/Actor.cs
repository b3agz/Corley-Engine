using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// A CorelyObject for actors. These are essentially NPCs, they can be interacted with and have dialogue.
/// </summary>
public class Actor : VisualObject {

    public Actor(string texturePath, Vector2 position) : base(texturePath, position) {
    }

}