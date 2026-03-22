using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;

namespace CorleyEngine.Core;

public class CorleyGame : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Scene _activeScene;
    private Entity _player;

    public CorleyGame() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {

        Log.Info("Initialising Engine...");

        // Test Code!!!

        // Make a new scene.
        _activeScene = new Scene();

        // Create a "player" for testing.
        _player = Entity.CreateStageEntity("Player", new(100, 100));
        _player.AddComponent(new CharacterController());
        _player.AddComponent(new PlayerInput());
        Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });
        _player.AddComponent(new SpriteRenderer(pixel));
        _player.Transform.CurrentScale = new Vector2(25, 50);

        // Add the "player" to the scene.
        _activeScene.AddEntity(_player);

        base.Initialize();
    }

    protected override void LoadContent() {

        Log.Info("Loading Content...");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

    }

    protected override void Update(GameTime gameTime) {

        // Time always needs updating for all the scripts that use it.
        Time.Update(gameTime);

        // Update the Input class so components get up to date information.
        Input.Update();

        // The active scene's Update function in turn calls Update on all of its entities.
        _activeScene.Update();

        base.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime) {

        // TODO: Let scene handle this? At least get the information from the active scene.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Make sure we don't try and draw before content has finished loading.
        if (_spriteBatch == null) return;

        _spriteBatch.Begin();

        // The active scene calls the draw method for any Entities that need it.
        _activeScene.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);

    }
}