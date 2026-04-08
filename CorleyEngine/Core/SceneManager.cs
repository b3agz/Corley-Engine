using System;
using System.IO;
using CorleyEngine.IO;

namespace CorleyEngine.Core;

/// <summary>
/// A static class responsible for loading, saving, and providing access to scenes in the currently loaded project.
/// </summary>
public static class SceneManager {

    /// <summary>
    /// Returns the currently active <see cref="Scene"/>.
    /// </summary>
    public static Scene ActiveScene { get; private set; }

    /// <summary>
    /// Creates a new <see cref="SceneData"/>, saves it as a .corleyscene file, and registers its path in the project dictionary.
    /// </summary>
    /// <param name="sceneName">The name of the scene.</param>
    /// <param name="relativeSubFolder">Optional: The relative path inside the Assets folder (eg, "Scenes", or "Data/Scenes").</param>
    public static void CreateScene(string sceneName, string relativeSubFolder = "") {

        if (ProjectManager.CurrentProject == null) {
            throw new InvalidOperationException("[SceneManager] Cannot create a scene without an active project.");
        }

        if (ProjectManager.CurrentProject.Scenes.ContainsKey(sceneName)) {
            throw new Exception($"[SceneManager] A scene named '{sceneName}' is already registered in this project.");
        }

        // Create the filename.
        string fileName = $"{sceneName}.corleyscene";

        // If a relative path has been provided, combine it with the filename, otherwise just use the filename.
        // Force forward-slashes so the JSON path is OS-agnostic.
        string relativeFilePath = string.IsNullOrWhiteSpace(relativeSubFolder)
            ? fileName
            : Path.Combine(relativeSubFolder, fileName).Replace("\\", "/");

        // Make the class, save it, and add it to the project.
        string absoluteFilePath = Path.Combine(ProjectManager.CurrentProject.AbsoluteAssetPath, relativeFilePath);
        SceneData newSceneData = new () { Name = sceneName };
        DataSerializer.Save(newSceneData, absoluteFilePath);
        ProjectManager.CurrentProject.AddScene(sceneName, relativeFilePath);
        ProjectManager.SaveProject();

    }

    /// <summary>
    /// Saves the currently active scene's data back to its exact file on disk.
    /// </summary>
    public static void SaveScene() {

        if (ProjectManager.CurrentProject == null) {
            throw new InvalidOperationException("[SceneManager] Cannot save scene without an active project.");
        }

        if (ActiveScene == null || ActiveScene.Data == null) {
            throw new InvalidOperationException("[SceneManager] No active scene is currently loaded to save.");
        }

        string sceneName = ActiveScene.Data.Name;

        // Get the relative scene file location from the project's Scene dictionary.
        if (!ProjectManager.CurrentProject.Scenes.TryGetValue(sceneName, out string relativeFilePath)) {
            throw new FileNotFoundException($"[SceneManager] Cannot save: Scene '{sceneName}' is not registered in the project dictionary.");
        }

        string absoluteFilePath = Path.Combine(ProjectManager.CurrentProject.AbsoluteAssetPath, relativeFilePath);
        DataSerializer.Save(ActiveScene.Data, absoluteFilePath);

        // Make sure we know that there are no unsaved changes for this scene.
        ActiveScene.Data.HasChanged = false;

    }

    /// <summary>
    /// Loads a scene by looking up <paramref name="sceneName"/> in <see cref="CorleyProject.Scenes"/> to get the relative path.
    /// </summary>
    public static void LoadScene(string sceneName) {

        Log.Info($"[SceneManager] Attempting to load scene '{sceneName}'");

        if (ProjectManager.CurrentProject == null) {
            throw new InvalidOperationException("[SceneManager] Cannot load a scene without an active project.");
        }

        // Look up the relative path using the scene name.
        if (!ProjectManager.CurrentProject.Scenes.TryGetValue(sceneName, out string relativeFilePath)) {
            throw new FileNotFoundException($"[SceneManager] Scene '{sceneName}' is not registered in the project dictionary.");
        }

        // TODO: Handle the graceful closing of any open scenes.

        string absoluteFilePath = Path.Combine(ProjectManager.CurrentProject.AbsoluteAssetPath, relativeFilePath);

        // Load the SceneData from the .corleyscene file in the Assets folder.
        SceneData loadedData = DataSerializer.Load<SceneData>(absoluteFilePath);

        // Create a new Scene instance, initialise it with the SceneData, and set it as the ActiveScene.
        Scene runtimeScene = new ();
        runtimeScene.Initialize(loadedData);

        // Rebuild the links between child entities and their parents.
        runtimeScene.RebuildHierarchy();

        ActiveScene = runtimeScene;

    }
}