using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;
using System;
using System.IO;

namespace CorleyEngine.Core;

public class CorleyRuntime : CorleyGame {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public CorleyRuntime() {

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


        SetWindowTitle("Corley Engine v0.1.0");

        base.LoadContent();

    }

    protected override void Update(GameTime gameTime) {

        base.Update(gameTime);

        // Update canvas and input so that camera renders properly and mouse cursor position is
        // reported accurately.
        Viewport viewport = GraphicsDevice.Viewport;
        BuildCanvas(viewport.Width, viewport.Height);
        Input.ViewportOffset = Vector2.Zero;
        Input.ViewportDisplaySize = new Vector2(viewport.Width, viewport.Height);
        Input.ViewportScale = Vector2.One;

        // Hand execution over to the loaded scene
        SceneManager.ActiveScene?.Update();

    }

    protected override void Draw(GameTime gameTime) {

        base.Draw(gameTime);

        _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
        _spriteBatch.Draw(GameCanvas, GraphicsDevice.Viewport.Bounds, Color.White);
        _spriteBatch.End();

    }

}