using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CorleyEngine.Core;

namespace CorleyEngine.Components;

// Temporary class for testing the basics.
public class ClickMover(Transform transform, float speed) : IComponent {

    private Vector2 _targetPosition;
    private bool _isMoving = false;

    /// <inheritdoc />
    void IComponent.Update() {
        if (transform == null) return;

        MouseState mouse = Mouse.GetState();

        if (Input.IsActionTriggered("Primary")) {
        _targetPosition = Input.CursorPosition;
        _isMoving = true;
    }

        if (_isMoving) {
            Vector2 direction = _targetPosition - transform.Position;
            float distance = direction.Length();

            float moveStep = speed * Time.DeltaTime;

            if (distance <= moveStep) {
                transform.Position = _targetPosition;
                _isMoving = false;
            }
            else {
                direction.Normalize();
                transform.Position += direction * moveStep;
            }
        }
    }
}