using CorleyEngine.Core;
using Microsoft.Xna.Framework;

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

            // Get the raw screen click
            Vector2 rawClick = Input.CursorPosition;

            // Translate it to World Space
            Vector2 worldTarget = Camera.MainCamera.ScreenToWorldSpace(rawClick);

            // Command the controller using the true world coordinates
            _controller.MoveTo(worldTarget);
        }

    }
}