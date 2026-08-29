using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace CorleyEngine.Core;

/// <summary>
/// Manages the rendering of visual objects, ensuring they are drawn in the correct order according
/// <see cref="CorleyObject.Depth"/>
/// </summary>
public static class RenderManager {

    private static readonly List<RenderableObject> _visualObjects = new();
    private static List<RenderableObject> _sortedObjects = new();
    private static bool _isDirty = true;

    /// <summary>
    /// Returns whether the RenderManager is currently dirty and needs the render list ordering.
    /// </summary>
    public static bool IsDirty => _isDirty;

    /// <summary>
    /// Called when a something has been changed that necessitates a sorting of the render list.
    /// </summary>
    public static void SetDirty() => _isDirty = true;

    /// <summary>
    /// Registers a visual object to be managed by the RenderManager.
    /// </summary>
    public static void Register(RenderableObject visualObject) {
        if (!_visualObjects.Contains(visualObject)) {
            _visualObjects.Add(visualObject);
            _isDirty = true;
        }
    }

    /// <summary>
    /// Unregisters a visual object.
    /// </summary>
    public static void Unregister(RenderableObject visualObject) {
        if (_visualObjects.Remove(visualObject)) {
            _isDirty = true;
        }
    }

    /// <summary>
    /// Draws all registered visual objects, sorted by their depth.
    /// </summary>
    public static void DrawAll() {
        if (_isDirty) {
            _sortedObjects = _visualObjects.OrderBy(o => o.Depth).ToList();
            _isDirty = false;
        }

        foreach (var obj in _sortedObjects) {
            obj.OnDraw();
        }
    }

    /// <summary>
    /// Gets all objects at a specific point.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <param name="foundObjects">The list of objects found at the point.</param>
    /// <returns>True if any objects were found, false otherwise.</returns>
    public static bool GetObjectsAtPoint(Vector2 point, out List<RenderableObject> foundObjects) {
        if (_isDirty) {
            _sortedObjects = _visualObjects.OrderBy(o => o.Depth).ToList();
            _isDirty = false;
        }

        foundObjects = new();
        foreach (var obj in _sortedObjects) {
            if (obj.IsPointInside(point)) {
                foundObjects.Add(obj);
            }
        }
        return foundObjects.Count > 0;
    }

    /// <summary>
    /// Gets the first object found at a specific point.
    /// </summary>
    /// <param name="point">The point to check.</param>
    /// <param name="foundObject">The object found at the point, if any.</param>
    /// <returns>True if an object was found, false otherwise.</returns>
    public static bool GetObjectAtPoint(Vector2 point, out RenderableObject? foundObject) {
        if (_isDirty) {
            _sortedObjects = _visualObjects.OrderBy(o => o.Depth).ToList();
            _isDirty = false;
        }

        foreach (var obj in _sortedObjects) {
            if (obj.IsPointInside(point)) {
                foundObject = obj;
                return true;
            }
        }

        foundObject = null;
        return false;
    }
}
