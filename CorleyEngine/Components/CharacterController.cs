using Microsoft.Xna.Framework;
using CorleyEngine.Core;

namespace CorleyEngine.Components;

/// <summary>
/// A simple character controller component.
/// </summary>
public class CharacterController : Component {

    /// <summary>
    /// Movement speed in pixels-per-second.
    /// </summary>
    public float MoveSpeed = 200f;

    private Vector2? _targetPosition = null;

    // TODO: Future Pathfinding
    // private Queue<Vector2> _currentPath = new();

    /// <summary>
    /// Gives the CharacterController a new position to move to.
    /// </summary>
    public void MoveTo(Vector2 destination) {
        _targetPosition = destination;
    }

    /// <summary>
    /// Halts all current movement.
    /// </summary>
    public void Stop() {
        _targetPosition = null;
        // _currentPath.Clear();
    }

    /// <inheritdoc />
    public override void Update() {

        if (Transform == null || _targetPosition == null) return;

        Vector2 direction = _targetPosition.Value - Transform.Position;
        float distance = direction.Length();
        float step = MoveSpeed * Time.DeltaTime;

        // Check if CharacterController has arrived at destination. If not, keep moving.
        if (distance <= step) {
            Transform.Position = _targetPosition.Value;
            Stop();
        } else {
            direction.Normalize();
            Transform.Position += direction * step;
        }
    }
}
