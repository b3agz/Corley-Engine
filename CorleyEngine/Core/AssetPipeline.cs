using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CorleyEngine.Core;

/// <summary>
/// The Editor-side pipeline that compiles raw media into xnb files and copies raw data.
/// This should NOT be included in a final shipped game build.
/// </summary>
/// <remarks>
/// Users will store their assets in an Assets folder. This system is responsible for converting
/// those assets and moving them to the Content folder, which is what MonoGame uses.
/// </remarks>
public static class AssetPipeline {

    /// <summary>
    /// Syncs the existing assets up with new
    /// </summary>
    public static void SyncAndBuild() {

        // Build paths.
        string runtimeDir = AppDomain.CurrentDomain.BaseDirectory;
        string outputContentDir = Path.Combine(runtimeDir, EnginePaths.COMPILED_CONTENT_FOLDER_NAME);

        string sourceDir = Path.GetFullPath(Path.Combine(runtimeDir, "../../../"));
        string sourceContentDir = Path.Combine(sourceDir, EnginePaths.COMPILED_CONTENT_FOLDER_NAME);

        string rawAssetsDir = Path.Combine(sourceDir, EnginePaths.ASSET_FOLDER_NAME);
        string mgcbFilePath = Path.Combine(sourceDir, EnginePaths.COMPILED_CONTENT_FOLDER_NAME, "Content.mgcb");

        // If the directories don't exist, create them.
        if (!Directory.Exists(rawAssetsDir)) Directory.CreateDirectory(rawAssetsDir);
        if (!Directory.Exists(outputContentDir)) Directory.CreateDirectory(outputContentDir);

        // Get the current Content.mgcb file.
        List<string> mgcbLines = File.Exists(mgcbFilePath) ? File.ReadAllLines(mgcbFilePath).ToList() : [];
        string mgcbText = File.Exists(mgcbFilePath) ? File.ReadAllText(mgcbFilePath) : "";
        bool needsSaving = false;

        // Get all the files from the Asset directory.
        string[] allFiles = Directory.GetFiles(rawAssetsDir, "*.*", SearchOption.AllDirectories);

        foreach (string file in allFiles) {
            string extension = Path.GetExtension(file).ToLower();
            string relativePath = Path.GetRelativePath(sourceContentDir, file).Replace("\\", "/");

            // Handle plain text data files.
            // TODO: Factor in other file extensions, maybe even custom extensions for this engine.
            if (extension == ".json" || extension == ".txt") {
                string destination = Path.Combine(outputContentDir, relativePath);

                // Respect folder structure in compiled assets.
                Directory.CreateDirectory(Path.GetDirectoryName(destination));

                if (!File.Exists(destination) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destination)) {
                    File.Copy(file, destination, overwrite: true);
                    Log.Info($"AssetPipeline - Copied data file: {relativePath}");
                }
                continue;
            }

            // Handle media files.
            // TODO: Add support for more media file types.
            if (!mgcbText.Contains(relativePath)) {
                string importer = "";
                string processor = "";

                switch (extension) {
                    case ".png":
                    case ".jpg":
                        importer = "TextureImporter";
                        processor = "TextureProcessor";
                        break;
                    case ".wav":
                        importer = "WavImporter";
                        processor = "SoundEffectProcessor";
                        break;
                    case ".spritefont":
                        importer = "FontDescriptionImporter";
                        processor = "FontDescriptionProcessor";
                        break;
                    default:
                        // TODO: Right now we just ignore unsupported types. Once we have an editor, we're going to want to notify the user that their file isn't supported.
                        continue;
                }

                mgcbLines.Add("");
                mgcbLines.Add($"#begin {relativePath}");
                mgcbLines.Add($"/importer:{importer}");
                mgcbLines.Add($"/processor:{processor}");

                // If an image, apply standard parameters.
                if (importer == "TextureImporter") {
                    mgcbLines.Add($"/processorParam:ColorKeyColor=255,0,255,255");
                    mgcbLines.Add($"/processorParam:ColorKeyEnabled=True");
                    mgcbLines.Add($"/processorParam:GenerateMipmaps=False");
                    mgcbLines.Add($"/processorParam:PremultiplyAlpha=True");
                }

                mgcbLines.Add($"/build:{relativePath}");
                needsSaving = true;
                Log.Info($"AssetPipeline - Added new media asset to pipeline: {relativePath}");
            }
        }

        // If media files have been added, compile the new files.
        if (needsSaving) {
            File.WriteAllLines(mgcbFilePath, mgcbLines);
            Log.Info("[Pipeline] Updated Content.mgcb file. Running compiler...");

            ProcessStartInfo processInfo = new() {
                FileName = "dotnet",
                // Force MGCB to output the compiled .xnb straight into the runtime bin folder
                Arguments = $"tool run mgcb /@:\"{mgcbFilePath}\" /outputDir:\"{outputContentDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = rawAssetsDir
            };

            using Process process = Process.Start(processInfo);
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(error)) {
                Log.Error($"AssetPipeline - MGCB COMPILATION FAILED: {error}");
            }
            else {
                Log.Info("AssetPipeline - Compilation complete.");
            }
        }
    }
}
