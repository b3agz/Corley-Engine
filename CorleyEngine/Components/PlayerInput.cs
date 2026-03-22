using CorleyEngine.Core;

namespace CorleyEngine.Components;

/// <summary>
/// Simple script for moving a CharacterController component around based on player input.
/// </summary>
public class PlayerInput : Component {

    private CharacterController _controller;

    public override void Awake() {

        _controller = Entity.GetComponent<CharacterController>();
        if (_controller == null) {
            Log.Error($"PlayerInput component was added to {Entity.Name} but no CharacterController component was found.");
        }

    }

    public override void Update() {

        if (_controller == null) return;

        if (Input.IsActionTriggered("Primary")) {
            _controller.MoveTo(Input.CursorPosition);
        }

    }
}