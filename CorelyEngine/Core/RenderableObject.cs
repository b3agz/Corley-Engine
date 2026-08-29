using System;
using System.Numerics;
using CorleyEngine.Core;

namespace CorleyEngine.Core;

/// <summary>
/// A base class for objects that are rendered to the screen and require a transform (Position, Scale, Rotation, Depth).
/// </summary>
public abstract class RenderableObject : CorleyObject {

    private Vector2 _position;

    /// <summary>
    /// The position of the object in the game world. Because we are working with pixels,
    /// and because we can't draw half-pixels, this value is automatically rounded to the
    /// nearest whole numbers when set.
    /// </summary>
    public Vector2 Position {
        get => _position;
        set {
            _position = new(MathF.Round(value.X), MathF.Round(value.Y));
        }
    }

    public Vector2 Scale { get; set; } = new(1f, 1f);
    public float Rotation { get; set; } = 0f;

    private uint _depth = 100;

    private bool _isUI;

    /// <summary>
    /// The depth or sorting order of this object. Modifying this value automatically calls <see cref="RenderManager.SetDirty();"/>
    /// </summary>
    /// <value></value>
    public uint Depth {
        get => _depth;
        set {
            if (_depth != value) {
                _depth = value;
                RenderManager.SetDirty();
            }
        }
    }

    protected RenderableObject(bool isUI = false) : base() {
        _isUI = isUI;
        AddToRenderer();
    }

    protected RenderableObject(Vector2 position, bool isUI = false) : base(position) {
        Position = position;
        _isUI = isUI;
        AddToRenderer();
    }

    public override void SetEnabled(bool isEnabled) {

        if (isEnabled) {
            AddToRenderer();
        }
        else {
            RemoveFromRenderer();
        }
        base.SetEnabled(isEnabled);
    }

    private void AddToRenderer() {
        if (!_isUI)
            RenderManager.Register(this);
        else
            UIRenderManager.Register(this);
    }

    private void RemoveFromRenderer() {
        if (!_isUI)
            RenderManager.Unregister(this);
        else
            UIRenderManager.Unregister(this);
    }

    /// <summary>
    /// Draws the object.
    /// </summary>
    public abstract void OnDraw();
}
