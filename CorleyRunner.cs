using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;
using System;
using System.IO;

namespace CorleyEngine.Core;

public class CorleyRunner : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public CorleyRunner() {

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

    }

    protected override void Initialize() {

        Log.Info("Starting Engine...");

        AssetManager.Initialise(ProjectManager.CurrentProject.AbsoluteAssetPath, GraphicsDevice);

        base.Initialize();

    }

    protected override void LoadContent() {

        // Since the game engine part uses data streaming to load standard media files, this section is exclusively
        // for engine-only content that has been compiled into .xnb files. The active scene will handle everything
        // else.

        Log.Info("Loading Binaries...");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SceneManager.LoadScene("TestLevel");

    }

    protected override void Update(GameTime gameTime) {

        // Time always needs updating for all the scripts that use it.
        Time.Update(gameTime);

        // Update the Input class so components get up to date information.
        Input.Update();

        // Hand execution over to the loaded scene
        SceneManager.ActiveScene?.Update();

        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime) {

        // TODO: Let scene handle this? At least get the information from the active scene.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Make sure we don't try and draw before content has finished loading.
        if (_spriteBatch == null) return;

        // The active scene calls the draw method for any Entities that need it.
        SceneManager.ActiveScene?.Draw(_spriteBatch);

        base.Draw(gameTime);

    }
}