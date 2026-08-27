using System;
using System.Collections.Generic;
using Raylib_cs;

namespace CorleyEngine.Core;

/// <summary>
/// Handles the loading, unloading, and caching of assets.
/// </summary>
public static class Assets {

    /// <summary>
    /// Dictionary to store loaded assets.
    /// </summary>
    private static readonly Dictionary<string, object> _loadedAssets = new();

    /// <summary>
    /// Loads or retrieves a texture.
    /// </summary>
    /// <param name="path">The path to the texture file.</param>
    /// <returns>The loaded texture.</returns>
    public static Texture2D LoadTexture(string path) {
        if (_loadedAssets.TryGetValue(path, out var asset)) {
            return (Texture2D)asset;
        }

        Texture2D texture = Raylib.LoadTexture(path);
        _loadedAssets[path] = texture;
        return texture;
    }

    /// <summary>
    /// Loads or retrieves a sound.
    /// </summary>
    /// <param name="path">The path to the sound file.</param>
    /// <returns>The loaded sound.</returns>
    public static Sound LoadSound(string path) {
        if (_loadedAssets.TryGetValue(path, out var asset)) {
            return (Sound)asset;
        }

        Sound sound = Raylib.LoadSound(path);
        _loadedAssets[path] = sound;
        return sound;
    }

    /// <summary>
    /// Unloads all assets from the cache and disposes of them properly.
    /// </summary>
    public static void UnloadAll() {
        foreach (var asset in _loadedAssets.Values) {
            if (asset is Texture2D texture) {
                Raylib.UnloadTexture(texture);
            }
            else if (asset is Sound sound) {
                Raylib.UnloadSound(sound);
            }
            else if (asset is IDisposable disposable) {
                disposable.Dispose();
            }
        }
        _loadedAssets.Clear();
    }

    /// <summary>
    /// Unloads an asset from the cache and disposes of it properly.
    /// </summary>
    /// <param name="path">The path to the asset.</param>
    public static void Unload(string path) {
        if (_loadedAssets.Remove(path, out var asset)) {
            if (asset is Texture2D texture) {
                Raylib.UnloadTexture(texture);
            }
            else if (asset is Sound sound) {
                Raylib.UnloadSound(sound);
            }
            else if (asset is IDisposable disposable) {
                disposable.Dispose();
            }
        }
    }
}
