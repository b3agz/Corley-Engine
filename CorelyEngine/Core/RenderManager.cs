using System.Collections.Generic;
using System.Linq;

namespace CorleyEngine.Core;

/// <summary>
/// Manages the rendering of visual objects, ensuring they are drawn in the correct order according
/// <see cref="CorleyObject.Depth"/>
/// </summary>
public static class RenderManager {

    private static readonly List<VisualObject> _visualObjects = new();

    /// <summary>
    /// Registers a visual object to be managed by the RenderManager.
    /// </summary>
    public static void Register(VisualObject visualObject) {
        if (!_visualObjects.Contains(visualObject)) {
            _visualObjects.Add(visualObject);
        }
    }

    /// <summary>
    /// Unregisters a visual object.
    /// </summary>
    public static void Unregister(VisualObject visualObject) {
        _visualObjects.Remove(visualObject);
    }

    /// <summary>
    /// Draws all registered visual objects, sorted by their depth.
    /// </summary>
    public static void DrawAll() {
        var sortedObjects = _visualObjects.OrderBy(o => o.Depth).ToList();
        foreach (var obj in sortedObjects) {
            obj.OnDraw();
        }
    }
}
