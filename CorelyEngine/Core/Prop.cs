using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// A CorelyObject for props. Props are interactable objects that are not actors.
/// </summary>
public class Prop : VisualObject {

    public Prop(string texturePath, Vector2 position) : base(texturePath, position) {
    }

}