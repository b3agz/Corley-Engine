using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CorleyEngine.Core;

/// <summary>
/// An abstract Game class that Runtime and Editor inherit from.
/// </summary>
public abstract class CorleyGame : Game {

    public RenderTarget2D GameCanvas;
    protected SpriteBatch _sceneBatch;

    public int RenderWidth { get; private set; } = 1920;
    public int RenderHeight { get; private set; } = 1080;

    protected override void Initialize() {

        GameCanvas = new RenderTarget2D(GraphicsDevice, RenderWidth, RenderHeight);
        _sceneBatch = new SpriteBatch(GraphicsDevice);

        // Catch unhandled C# exceptions and push them to our visual console
        AppDomain.CurrentDomain.UnhandledException += (sender, args) => {
            if (args.ExceptionObject is Exception ex) {
                EngineLogger.Error($"[FATAL C# CRASH] {ex.Message}\n{ex.StackTrace}");
            }
        };

        base.Initialize();

    }

    protected override void LoadContent() {

        SceneManager.LoadScene(ProjectManager.CurrentProject.DefaultScene);

        base.LoadContent();

    }

    protected override void Update(GameTime gameTime) {

        // TODO: By default, Time will operate from when the editor launched. It should run (and reset) when play mode is initiated.
        Time.Update(gameTime);

        // We want to get the game input regardless. Game input is tied to the game view, and we'll need
        // relative cursor positions to be able to move objects.
        Input.Update();

    }

    protected override void Draw(GameTime gameTime) {

        // Tell MonoGame to draw to our internal canvas.
        GraphicsDevice.SetRenderTarget(GameCanvas);
        GraphicsDevice.Clear(Color.Black);

        // If we have an active scene, tell it to run its draw function.
        SceneManager.ActiveScene?.Draw(_sceneBatch);

        // Unset the canvas so the inheriting class can draw the final
        // result to screen.
        GraphicsDevice.SetRenderTarget(null);

        base.Draw(gameTime);
    }

    /// <summary>
    /// Creates a new canvas at the specified size.
    /// </summary>
    /// <param name="width">The width of the canvas.</param>
    /// <param name="height">The height of the canvas.</param>
    public void BuildCanvas(int width, int height) {

        // If the width or height are invalid, return.
        if (width <= 0 || height <= 0) return;

        // If the width and height haven't changed and we already have a canvas, we don't
        // need to do anything.
        if (width == RenderWidth && height == RenderHeight && GameCanvas != null) return;

        // Clean up the old one to prevent memory leaks
        GameCanvas?.Dispose();

        RenderWidth = width;
        RenderHeight = height;
        GameCanvas = new RenderTarget2D(GraphicsDevice, RenderWidth, RenderHeight);

        // Push the new dimensions to the global Screen class
        GameView.UpdateDimensions(width, height);

    }

    /// <summary>
    /// Sets the title of the game window.
    /// </summary>
    /// <param name="title">String containing the game title.</param>
    public virtual void SetWindowTitle(string title = "Corley Engine") {
        Window.Title = title;
    }

}