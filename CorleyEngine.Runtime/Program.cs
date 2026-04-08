using System;
using System.IO;
using CorleyEngine.Core;
using System.Text.Json;

namespace CorleyEngine;

internal class Program {

    [STAThread]
    static void Main(string[] args) {

        // TODO: Get rid of the fallback string. The engine should be provided a valid project location when launched, but at this stage in development, this is easier.
        string inputPath = Helpers.GetEnginePath() + @"CorleyEngine.Runtime\SampleProject\Sample Project.corleyproject";

        if (args.Length > 0) {
            inputPath = args[0];
        }

        string projectFilePath = string.Empty;

        // Run a validation check on the project folder. If the folder doesn't exist or doesn't have a .corleyproject file
        // inside it, there's no point in firing up the engine because it will just throw errors anyway.

        // If the path is a directory, we need to check for a .corleyproject file in the root of the directory.
        if (Directory.Exists(inputPath)) {

            string[] projectFiles = Directory.GetFiles(inputPath, "*.corleyproject");

            // If there's no .corelyproject file, the project folder is invalid.
            if (projectFiles.Length == 0) {
                ShowLaunchError($"No .corleyproject file found in the provided directory:\n{inputPath}");
                return;
            }

            // If there are multiple .corelyproject files, we need to know which one to use.
            // (there shouldn't be multiple project files)
            if (projectFiles.Length > 1) {
                ShowLaunchError($"Multiple .corleyproject files found. Please specify the exact file to boot:\n{inputPath}");
                return;
            }

            projectFilePath = projectFiles[0];

            // If the path leads to a file, we need to
        }
        else if (File.Exists(inputPath)) {

            // They passed a specific file. Make sure it's actually a Corley Project.
            if (!inputPath.EndsWith(".corleyproject", StringComparison.OrdinalIgnoreCase)) {
                ShowLaunchError($"The target file is not a valid Corley Project:\n{inputPath}");
                return;
            }

            projectFilePath = inputPath;

        }
        else {

            ShowLaunchError($"The specified path does not exist:\n{inputPath}");
            return;

        }

        // If we have a valid project folder, we still want to make sure the project file is valid before launching
        // the engine proper. We can do that by passing it to the ProjectManager and catching any exceptions.
        try {

            ProjectManager.LoadProject(projectFilePath);

        } catch (JsonException jsonEx) {

            ShowLaunchError($"The project file is corrupted or contains invalid JSON.\nPath: {projectFilePath}\nDetails: {jsonEx.Message}");
            return;

        } catch (Exception ex) {

            ShowLaunchError($"An unexpected error occurred while loading the project file.\nPath: {projectFilePath}\nDetails: {ex.Message}");
            return;

        }

        // if we get through all those checks, we know we have a valid project file. Launch the engine.
        using CorleyRuntime game = new();
        game.Run();

    }

    /// <summary>
    /// Displays an error message in the console/command prompt and waits for the user
    /// to acknowledge it before continuing.
    /// </summary>
    /// <param name="message">The message to display.</param>
    private static void ShowLaunchError(string message) {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[ENGINE BOOT FAILURE]");
        Console.WriteLine(message);
        Console.ResetColor();

        // Attempting to ReadKey in an IDE environment like the VS Code terminal throws an exception,
        // so try it. If it fails, just let the method end.
        try {
            if (!Console.IsInputRedirected) {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
        catch { }

    }
}