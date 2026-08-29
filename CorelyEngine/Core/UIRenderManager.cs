using System.Collections.Generic;
using System.Linq;

namespace CorleyEngine.Core;

/// <summary>
/// Manages the rendering of UI objects, ensuring they are drawn on top of the scene
/// and are unaffected by camera translation.
/// </summary>
public static class UIRenderManager {

    private static readonly List<RenderableObject> _uiObjects = new();
    private static List<RenderableObject> _sortedObjects = new();
    private static bool _isDirty = true;

    public static void SetDirty() => _isDirty = true;

    /// <summary>
    /// Registers a UI object to be managed by the UIRenderManager.
    /// </summary>
    public static void Register(RenderableObject uiObject) {
        if (!_uiObjects.Contains(uiObject)) {
            _uiObjects.Add(uiObject);
            _isDirty = true;
        }
    }

    /// <summary>
    /// Unregisters a UI object.
    /// </summary>
    public static void Unregister(RenderableObject uiObject) {
        if (_uiObjects.Remove(uiObject)) {
            _isDirty = true;
        }
    }

    /// <summary>
    /// Draws all registered UI objects, sorted by their depth.
    /// </summary>
    public static void DrawAll() {
        if (_isDirty) {
            _sortedObjects = _uiObjects.OrderBy(o => o.Depth).ToList();
            _isDirty = false;
        }

        foreach (var obj in _sortedObjects) {
            obj.OnDraw();
        }
    }
}
