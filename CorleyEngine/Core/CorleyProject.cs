using System.Text.Json.Serialization;

namespace CorleyEngine.Core;

/// <summary>
/// Stores information about Corley project.
/// </summary>
public class CorleyProject {

    /// <summary>
    /// The name of the project as shown in the title bar of the game window.
    /// </summary>
    public string ProjectName { get; set; } = "Untitled Project";

    /// <summary>
    /// Versioning information. Just a string so the user can version however they like.
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// The name of the assets folder.
    /// </summary>
    public string AssetDirectoryName = "Assets";

    /// <summary>
    /// The absolute path to the project files. This is ignored by JSON serialisation in favour
    /// of constructing the path at load time, so the project doesn't break if the user moves
    /// their project folder.
    /// </summary>
    [JsonIgnore]
    public string AbsoluteAssetPath { get; set; }

}