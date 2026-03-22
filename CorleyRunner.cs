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

        // Test Code!!!

        // Make a new scene.
        _activeScene = new Scene();

        // Create a "player" entity and give it some components.
        _player = new Entity("Player Box");
        Transform transform = new(new Vector2(100, 100));
        _player.AddComponent(transform);
        _player.AddComponent(new ClickMover(transform, 300f));

        // Add the "player" to the scene.
        _activeScene.AddEntity(_player);

        base.Initialize();
    }

    protected override void LoadContent() {

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create a placeholder white box texture for the player.
        Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        // Add a SpriteRenderer component to the player with the placeholder texture.
        _player.AddComponent(new SpriteRenderer(pixel));

        // Bump the scale of the player's transform so we can see it.
        _player.GetComponent<Transform>().CurrentScale = new Vector2(50, 50);

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