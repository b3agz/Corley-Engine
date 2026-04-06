using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;
using System;
using System.IO;

namespace CorleyEngine.Core;

public class CorleyRunner : CorleyGame {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public CorleyRunner() {

        _graphics = new GraphicsDeviceManager(this);
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

        base.Draw(gameTime);

        // TODO: Compensate for canvas when getting mouseclick position.

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        _spriteBatch.Draw(_gameCanvas, GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.End();

        //base.Draw(gameTime);

    }
}