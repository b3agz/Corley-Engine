using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// A CorelyObject for scenery. Scenery is a static visual object that exists in the scene but
/// has no logic.
/// </summary>
public class Scenery : SpriteObject {

    public Scenery(string texturePath, Vector2 position) : base(texturePath, position) {
    }

}