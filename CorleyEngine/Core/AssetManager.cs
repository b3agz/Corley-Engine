using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;

namespace CorleyEngine.Core;

/// <summary>
/// Centralized manager for loading both compiled media (.xnb) and raw data (.json).
/// </summary>
public static class AssetManager {

    private const string INTERNAL_CONTENT_DIRECTORY = "InternalContent";

    // Internal engine assets.
    private static ContentManager _engineContent;

    // Game assets.
    private static ContentManager _gameContent;

    // Cache to prevent instances of the same asset being loaded several times.
    private static Dictionary<string, object> _loadedMediaCache = [];

    /// <summary>
    /// Initialises the AssetManager, must be called before any attempt to load content.
    /// </summary>
    public static void Initialize(IServiceProvider services, string gameDirectory = "Content") {
        _engineContent = new ContentManager(services, INTERNAL_CONTENT_DIRECTORY);
        _gameContent = new EngineContentManager(services, gameDirectory);
    }

    /// <summary>
    /// Loads media (textures, audio, fonts) using MonoGame's pipeline.
    /// </summary>
    /// <typeparam name="T">The type of media being loaded.</typeparam>
    /// <param name="assetName">The name of the asset, should NOT include filename extensions.</param>
    /// <returns>Returns an object of type <typeparamref name="T"/>, returns default if asset was not found.</returns>
    public static T LoadMedia<T>(string assetName) {

        // Check the cache to make sure we haven't already loaded this asset.
        if (_loadedMediaCache.TryGetValue(assetName, out object cachedAsset))
            return (T)cachedAsset;

        // Try and load the asset via MonoGame's pipeline. If found, cache it and return it.
        try {

            T asset = _gameContent.Load<T>(assetName);
            _loadedMediaCache.Add(assetName, asset);
            return asset;

        } catch (ContentLoadException) {

            // TODO: Fallback to some kind of default return (like Unity's godawful eye-searing pink).
            // Calculate the exact absolute path the engine was trying to read
            string rootPath = Path.GetFullPath(_gameContent.RootDirectory);
            string expectedFile = Path.Combine(rootPath, assetName + ".xnb");

            Log.Error($"[AssetManager] Could not find media asset: {assetName} at {expectedFile}");

            return default;

        }
    }

    /// <summary>
    /// Loads internal engine media. This is media that will be present regardless of the game or its conmtent files.
    /// </summary>
    /// <typeparam name="T">The type of media being loaded.</typeparam>
    /// <param name="assetName">The name of the asset, should NOT include filename extensions.</param>
    /// <returns>Returns an object of type <typeparamref name="T"/>, returns default if asset was not found.</returns>
    public static T LoadEngineMedia<T>(string assetName) {

        try {

            return _engineContent.Load<T>(assetName);

        }
        catch (ContentLoadException) {

            Log.Error($"[AssetManager] Could not find INTERNAL engine asset: {assetName}");
            return default;

        }
    }

    /// <summary>
    /// Loads raw text/data JSON-formatted files directly from the output directory and attempts to parse it.
    /// into <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data being loaded.</typeparam>
    /// <param name="fileName">The name of the asset, MUST include filename.</param>
    /// <remarks>
    /// Text files are not converted to xnb format like media files.
    /// </remarks>
    public static T LoadData<T>(string fileName) {

        string path = Path.Combine(_gameContent.RootDirectory, fileName);

        if (File.Exists(path)) {
            try {

                string jsonText = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(jsonText);

            } catch (JsonException ex) {

                Log.Error($"[AssetManager] Failed to parse JSON in {fileName}: {ex.Message}");
                return default;

            }
        }

        Log.Error($"[AssetManager] Could not find data file: {path}");
        return default;
    }

    /// <summary>
    /// Clears the media cache and unloads the game content (eg, when starting a new project).
    /// </summary>
    public static void UnloadAll() {
        _gameContent.Unload();
        _loadedMediaCache.Clear();
    }

}
