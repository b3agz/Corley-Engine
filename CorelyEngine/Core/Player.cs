using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// The actual player object.
/// </summary>
public class Player : SpriteObject {

    public Player(string texturePath, Vector2 position) : base(texturePath, position) {
    }

}