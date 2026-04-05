using System;
using System.IO;
using System.Text.Json;
using CorleyEngine.IO;

namespace CorleyEngine.Core;

/// <summary>
/// Handles loading and saving project information, as well as providing information about the project to
/// the rest of the engine.
/// </summary>
public static class ProjectManager {

    /// <summary>
    /// The currently loaded game project.
    /// </summary>
    public static CorleyProject CurrentProject { get; private set; }

    public static string ProjectRootDirectory { get; private set; }

    /// <summary>
    /// Loads a .corleyproject file from an absolute path and sets it as the active project.
    /// </summary>
    public static void LoadProject(string absoluteFilePath) {

        Log.Info($"Attempting to load project from \"{absoluteFilePath}\"...");

        CurrentProject = DataSerializer.Load<CorleyProject>(absoluteFilePath);

        ProjectRootDirectory = Path.GetDirectoryName(absoluteFilePath);
        CurrentProject.AbsoluteAssetPath = Path.Combine(ProjectRootDirectory, CurrentProject.AssetDirectoryName);

        if (!Directory.Exists(CurrentProject.AbsoluteAssetPath)) {
            Directory.CreateDirectory(CurrentProject.AbsoluteAssetPath);
        }

    }

    /// <summary>
    /// Saves the current project state to disk. Operation ensures only one .corleyproject file exists in the project
    /// folder. If another is present, this method either overwrites it (if it is the same name) or deletes it.
    /// </summary>
    /// <summary>
    public static void SaveProject() {

        if (CurrentProject == null) {
            throw new InvalidOperationException("[ProjectManager] Cannot save: No active project is loaded.");
        }

        if (string.IsNullOrWhiteSpace(ProjectRootDirectory)) {
            throw new InvalidOperationException("[ProjectManager] Cannot save: Project root directory is unknown.");
        }

        string targetFileName = $"{CurrentProject.ProjectName}.corleyproject";
        string targetFilePath = Path.Combine(ProjectRootDirectory, targetFileName);

        // Find every .corleyproject file currently sitting in the folder
        string[] existingProjectFiles = Directory.GetFiles(ProjectRootDirectory, "*.corleyproject");

        // Check each existing .corleyproject file. If it has a different name to the one we're about to
        // save, delete it.
        foreach (string file in existingProjectFiles) {
            if (!file.Equals(targetFilePath, StringComparison.OrdinalIgnoreCase)) {
                File.Delete(file);
            }
        }

        // Delegate the actual serialization and file writing to the DataSerializer
        DataSerializer.Save(CurrentProject, targetFilePath);

    }
}