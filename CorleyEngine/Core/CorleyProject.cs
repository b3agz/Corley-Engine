using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;

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

    // Key: Scene Name | Value: Relative Path (e.g., "Levels/Level_01.corleyscene")

    /// <summary>
    /// A dictionary of scenes in this project. The key is the scene name and the value is the relative
    /// path to the scene file.
    /// </summary>
    /// <remarks>This dictionary constitutes the scenes that actually
    /// ship. There could be other scene files in the project's asset folder, but, if they're
    /// not in this dictionary, they're not accessible to the rest of the project.
    /// </remarks>
    public Dictionary<string, string> Scenes { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns the name of the default scene in this project.
    /// </summary>
    public string DefaultScene { get; set; } = string.Empty;

    /// <summary>
    /// The absolute path to the project files. This is ignored by JSON serialisation in favour
    /// of constructing the path at load time, so the project doesn't break if the user moves
    /// their project folder.
    /// </summary>
    [JsonIgnore]
    public string AbsoluteAssetPath { get; set; }

    /// <summary>
    /// Adds a scene to the project's scene dictionary. If <see cref="DefaultScene"/> is not set, <paramref name="sceneName"/>
    /// will be set as the default scene.
    /// </summary>
    /// <param name="sceneName">The name of the scene as it appears in the editor.</param>
    /// <param name="relativeFilePath">The relative path to the scene file (inside the Assets folder).</param>
    public void AddScene(string sceneName, string relativeFilePath) {

        if (Scenes.ContainsKey(sceneName)) {
            Log.Info($"[CorleyProject] Attempted to add {sceneName} to project but a scene by that name already exists.");
            return;
        }

        Scenes.Add(sceneName, relativeFilePath);

        // If this project does not currently have a default scene, set this scene as the default.
        if (string.IsNullOrEmpty(DefaultScene))
            DefaultScene = sceneName;

    }

}