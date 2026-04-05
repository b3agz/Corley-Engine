using System;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace CorleyEngine.Core;

/// <summary>
/// A wrapper for MonoGame's ContentManager to make it easier to avoid MonoGame's rigid
/// path structure.
/// </summary>
public class EngineContentManager : ContentManager {

    private readonly string _absoluteRootPath;

    public EngineContentManager(IServiceProvider serviceProvider, string absolutePath)

        // Feed the base class a dummy string so it doesn't crash.
        : base(serviceProvider, "DummyString") {
        _absoluteRootPath = absolutePath;
    }

    protected override Stream OpenStream(string assetName) {

        // Use our unmodified absolute string, not MonoGame's RootDirectory
        string fullPath = Path.Combine(_absoluteRootPath, assetName + ".xnb");

        if (File.Exists(fullPath)) {
            return File.OpenRead(fullPath);
        }

        throw new ContentLoadException($"[EngineContentManager] Failed to load content at: {fullPath}");
    }
}