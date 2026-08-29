using System.Numerics;
using Raylib_cs;

namespace CorleyEngine.Core;

/// <summary>
/// A wrapper for Raylib's Camera2D, allowing for camera movement, rotation, and zooming.
/// </summary>
public class Camera {

    private Camera2D _camera2D;

    public Camera() {
        _camera2D = new Camera2D {
            Target = Vector2.Zero,
            Offset = Vector2.Zero,
            Rotation = 0.0f,
            Zoom = 1.0f
        };
    }

    public Vector2 Position {
        get => _camera2D.Target;
        set => _camera2D.Target = value;
    }

    public float Rotation {
        get => _camera2D.Rotation;
        set => _camera2D.Rotation = value;
    }

    public float Zoom {
        get => _camera2D.Zoom;
        set => _camera2D.Zoom = value;
    }

    public Vector2 Offset {
        get => _camera2D.Offset;
        set => _camera2D.Offset = value;
    }

    public Camera2D GetRaylibCamera() => _camera2D;
}
