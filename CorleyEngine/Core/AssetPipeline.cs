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

        // The absolute path to the folder where the .exe is currently running.
        string runtimeDir = AppDomain.CurrentDomain.BaseDirectory;

        // The destination folder where the running game looks for compiled (.xnb) assets.
        string outputContentDir = Path.Combine(runtimeDir, EnginePaths.CONTENT_FOLDER_NAME);

        // Navigates from the build folder back to the project root directory (where the .csproj and source code live).
        string sourceDir = Path.GetFullPath(Path.Combine(runtimeDir, "../../../"));

        // The 'Content' folder inside the project source, essentially a directory of asset files.
        string sourceContentDir = Path.Combine(sourceDir, EnginePaths.CONTENT_FOLDER_NAME);

        // The source folder for raw, uncompiled media files (png, wav, etc.) that the developer interacts with.
        string rawAssetsDir = Path.Combine(sourceDir, EnginePaths.ASSET_FOLDER_NAME);

        // The path to Content.mgcb so MonoGame knows what assets it has access to.
        string mgcbFilePath = Path.Combine(sourceDir, EnginePaths.CONTENT_FOLDER_NAME, "Content.mgcb");

        // Make sure the directories exist to avoid "Path Not Found" errors later.
        if (!Directory.Exists(rawAssetsDir)) Directory.CreateDirectory(rawAssetsDir);
        if (!Directory.Exists(outputContentDir)) Directory.CreateDirectory(outputContentDir);

        // Read Content.mgcb into memory. If there isn't one, initialise an empty list and start from scratch.
        List<string> mgcbLines = File.Exists(mgcbFilePath) ? File.ReadAllLines(mgcbFilePath).ToList() : [];

        // We only want to save Content.mgcb if there have been changes, so keep track of if there have been changes.
        bool needsSaving = false;

        // Go through the files and prune out anything that has been removed since the last time.
        List<string> cleanMgcbLines = [];
        bool skippingDeadBlock = false;

        // Check each line in Content.mcgb
        foreach (string line in mgcbLines) {

            // Each new asset in the files begins with "#begin", so we can just ignore any lines that don't.
            if (line.StartsWith("#begin ")) {

                string expectedRelativePath = line.Substring(7).Trim();
                string absoluteSourcePath = Path.GetFullPath(Path.Combine(sourceContentDir, expectedRelativePath));

                // If the file has been removed from the assets folder, we need to remove it from the Contents.
                // Renamed/removed files are essentially treated as deleted files. We remove them here and add
                // them as new files in the next phase.
                if (!File.Exists(absoluteSourcePath)) {

                    skippingDeadBlock = true;
                    needsSaving = true;

                    // Get the name of the compiled asset file.
                    string assetName = Path.GetFileNameWithoutExtension(expectedRelativePath);

                    // Find the compiled .xnb file in the Contents/bin folder. If it exists, delete it.
                    string xnbPath = Path.Combine(sourceContentDir, "bin/DesktopGL", assetName + ".xnb");
                    if (File.Exists(xnbPath)) File.Delete(xnbPath);

                    // Find the .mgcontent file in the Contents/obj folder. If it exits, delete it.
                    string mgContentPath = Path.Combine(sourceContentDir, "obj/DesktopGL", assetName + ".mgcontent");
                    if (File.Exists(mgContentPath)) File.Delete(mgContentPath);

                    Log.Info($"[AssetPipeline] Cleaned up artifacts for deleted asset: {xnbPath}");
                    continue;

                } else {

                    skippingDeadBlock = false;

                }

            }

            // skippingDeadBlock is only ever set to false at the end of a complete iteration, so checking here allows us to
            // ignore an entire block that we marked as dead on the first line because skippingDeadBlock will remain true until
            // we encounter a new block that isn't dead.
            if (skippingDeadBlock && (line.StartsWith('/') || string.IsNullOrWhiteSpace(line))) {
                continue;
            }

            skippingDeadBlock = false;
            cleanMgcbLines.Add(line);

        }

        mgcbLines = cleanMgcbLines;
        string mgcbText = string.Join(Environment.NewLine, mgcbLines);

        // Next, go through the files and add anything that's not currently in the Content file.
        string[] allFiles = Directory.GetFiles(rawAssetsDir, "*.*", SearchOption.AllDirectories);

        // Loop through every file in the asset directory.
        foreach (string file in allFiles) {

            string extension = Path.GetExtension(file).ToLower();
            string relativePath = Path.GetRelativePath(sourceContentDir, file).Replace("\\", "/");

            // If the file is plain text, it doesn't need converting, we can just copy it to the Content folder.
            // TODO: Factor in other file extensions, maybe even custom extensions for this engine.
            if (extension == ".json" || extension == ".txt") {

                string destination = Path.Combine(outputContentDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destination));

                // If the file already exists, check if the raw file in the asset folder was updated more recently.
                // If the asset folder version is more recent, overwrite the Content folder version.
                if (!File.Exists(destination) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destination)) {
                    File.Copy(file, destination, overwrite: true);
                    Log.Info($"[AssetPipeline] Copied data file: {relativePath}");
                }
                continue;
            }

            // If an asset file exists both as a raw asset and as an xnb binary, check if the last write time's match.
            // if they don't, a file might have been modified or replaced with a different file with the same name. In
            // either case, we need to make sure we rebuild the content library.
            string assetName = Path.GetFileNameWithoutExtension(file);
            string xnbPath = Path.Combine(sourceContentDir, "bin/DesktopGL", assetName + ".xnb");

            // If the xnb file exists, check if the raw asset is newer.
            if (File.Exists(xnbPath)) {
                DateTime rawTime = File.GetLastWriteTime(file);
                DateTime xnbTime = File.GetLastWriteTime(xnbPath);

                if (rawTime > xnbTime) {
                    needsSaving = true; // Trigger the compiler!
                    Log.Info($"[AssetPipeline] Detected edit in {relativePath}. Triggering rebuild.");
                }

            } else {
                // If the .xnb doesn't exist at all but the asset file does, we definitely need to rebuild the library.
                needsSaving = true;
            }

            // MonoGame requires media files like pngs and wavs to be converted into xnb binaries. First, check to make sure
            // Content.mgcb doesn't already contain the file (if it does, we don't need to add it).
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

                // Construct asset parameters.
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
                Log.Info($"[AssetPipeline] Added new media asset to pipeline: {xnbPath}");
            }
        }

        // If changes have been made (we've set needsSaving to true), then save.
        if (needsSaving) {

            // Writing the new Content.mcgb file is easy enough. The next bit, on the other hand...
            File.WriteAllLines(mgcbFilePath, mgcbLines);
            Log.Info("[AssetPipeline] Updated Content.mgcb file. Running compiler...");

            ProcessStartInfo processInfo = new() {
                FileName = "dotnet",
                Arguments = $"tool run mgcb /@:\"{mgcbFilePath}\"",
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
                Log.Error($"[AssetPipeline] MGCB COMPILATION FAILED: {error}");
            }
            else {
                Log.Info("[AssetPipeline] Compilation complete.");
            }

            // Inside your SyncAndBuild method, where you run the process:
// ProcessStartInfo processInfo = new() {
//     FileName = "dotnet",
//     // Use /@ to point to the response file.
//     // If we are standing in the 'Content' folder, we just need the filename.
//     Arguments = "tool run mgcb /@:Content.mgcb",

//     // CRITICAL: This MUST be the folder containing 'Content.mgcb'.
//     // If this is wrong, "../Assets/image.png" will never resolve.
//     WorkingDirectory = sourceContentDir,

//     RedirectStandardOutput = true,
//     RedirectStandardError = true,
//     UseShellExecute = false,
//     CreateNoWindow = true
// };

// using Process process = Process.Start(processInfo);

// // Read the ACTUAL output from the compiler
// string output = process.StandardOutput.ReadToEnd();
// string error = process.StandardError.ReadToEnd();
// process.WaitForExit();

// // Now we can actually see why it's not building
// if (!string.IsNullOrWhiteSpace(output)) Log.Info($"[MGCB Output]: {output}");
// if (!string.IsNullOrWhiteSpace(error)) Log.Error($"[MGCB Error]: {error}");

//             if (process.ExitCode == 0) {
//                 Log.Info("AssetPipeline - MGCB Process Finished.");
//             }
//             else {
//                 Log.Error($"AssetPipeline - MGCB Failed with Exit Code: {process.ExitCode}");
//             }


        }
    }
}
