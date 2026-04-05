using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CorleyEngine.Core;

public static class AssetManager {

    private static GraphicsDevice _graphicsDevice;
    private static string _assetsPath;

    // Simple cache directory to prevent the need to load assets in that we've already loaded.
    private static readonly Dictionary<string, object> _cache = new();

    /// <summary>
    /// Initialises the AssetManager, must be called before any attempt to load content.
    /// </summary>
    /// <param name="absoluteAssetPath">The absolute path to the assets folder of the project we are initialising.</param>
    /// <param name="device">MonoGame graphics device.</param>
    public static void Initialise(string absoluteAssetPath, GraphicsDevice device) {

        Log.Info("Initialising AssetManager...");
        _graphicsDevice = device;
        _assetsPath = absoluteAssetPath;

    }

    /// <summary>
    /// Attempts to get an asset from the assets folder.
    /// </summary>
    /// <typeparam name="T">The type of asset we expect to get.</typeparam>
    /// <param name="relativePath">The relative path to the asset.</param>
    /// <returns>An asset of type <typeparamref name="T"/>.</returns>
    /// <exception cref="FileNotFoundException">Throws exception if the requested file was not present at the expected location.</exception>
    /// <exception cref="NotSupportedException">Throws exception if the requested file was not of the expected Type.</exception>
    public static T Get<T>(string relativePath) where T : class {

        // If we already loaded the asset file, we can just get it from the cache.
        if (_cache.TryGetValue(relativePath, out var asset))
            return (T)asset;

        // Construct the full path to the asset.
        string fullPath = Path.Combine(_assetsPath, relativePath);

        if (!File.Exists(fullPath)) {
            throw new FileNotFoundException($"[AssetManager] User missing asset: {fullPath}");
        }

        object loadedAsset = null;

        // Text files do not need processing, we can just return the contents of the file.
        if (typeof(T) == typeof(string)) {
            loadedAsset = File.ReadAllText(fullPath);
            _cache[relativePath] = loadedAsset;
            return (T)loadedAsset;
        }

        using var stream = File.OpenRead(fullPath);

        // Try to get the asset file. Use try/catch so we can report our own error messages rather than
        // pipe MonoGame's out to the user.
        try {

            if (typeof(T) == typeof(Texture2D)) {

                loadedAsset = Texture2D.FromStream(_graphicsDevice, stream);

            }
            else if (typeof(T) == typeof(SoundEffect)) {

                loadedAsset = SoundEffect.FromStream(stream);

            }
            else {

                throw new NotSupportedException($"[AssetManager] Type {typeof(T)} is not supported for raw loading.");

            }

        }
        catch (InvalidOperationException ex) {

            throw new Exception($"[AssetManager] The file '{relativePath}' could not be loaded. It may be corrupted or saved in an unsupported format.", ex);

        }
        catch (ArgumentException ex) {

            throw new Exception($"[AssetManager] The file '{relativePath}' has invalid dimensions or data.", ex);

        }
        catch (Exception ex) {

            throw new Exception($"[AssetManager] An unexpected error occurred while reading '{relativePath}': {ex.Message}", ex);

        }

        // If we make it past all the error checks, throw the loaded asset into the cache before returning it so we don't have to load it next time.
        if (loadedAsset != null) {
            _cache[relativePath] = loadedAsset;
            return (T)loadedAsset;
        }

        // TODO: Return some kind of default (eg, Unity's searing pink texture).
        return null;

    }

    /// <summary>
    /// Clears the asset cache.
    /// </summary>
    public static void Unload() {

        foreach (object asset in _cache.Values) {
            if (asset is IDisposable disposable) disposable.Dispose();
        }
        _cache.Clear();

    }
}