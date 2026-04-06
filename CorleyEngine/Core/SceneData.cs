using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace CorleyEngine.Core;

/// <summary>
/// The serializable class for Scenes that can be saved as JSON data.
/// </summary>
public class SceneData {

    /// <summary>
    /// The name of the scene as it appears in the editor and references when switching scenes.
    /// </summary>
    public string Name { get; set; } = "New Scene";

    /// <summary>
    /// A list of entities this scene contains.
    /// </summary>
    public List<Entity> Entities { get; set; } = [];

    /// <summary>
    /// A bool to keep track of whether data has been changed. This should be set to false any time
    /// the SceneData is saved to disk, and marked as true any time details are changed.
    /// </summary>
    [JsonIgnore]
    public bool HasChanged = false;

}