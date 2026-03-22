using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;

namespace CorleyEngine.Core;

public class CorleyGame : Game {

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Scene _activeScene;

    public CorleyGame() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize() {

        Log.Info("Initialising Engine...");

        // Make a new scene.
        _activeScene = new Scene();

        base.Initialize();

    }

    protected override void LoadContent() {

        Log.Info("Loading Content...");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Temporary background image for camera testing.
        Texture2D bgImage = Content.Load<Texture2D>("tempbg");
        Entity background = Entity.CreateStageEntity("Background", new(-560, -240));
        background.AddComponent(new SpriteRenderer(bgImage));
        _activeScene.AddEntity(background);

        // Create a moveable "player" for testing.
        Entity player = Entity.CreateStageEntity("Player", new(100, 100));
        player.AddComponent(new CharacterController());
        player.AddComponent(new PlayerInput());

        // Load in a placeholder player sprite and set its pivot point to
        // the centre of the sprite.
        Texture2D playerImage = Content.Load<Texture2D>("PlaceholderMan");
        SpriteRenderer playerRenderer = new(playerImage);
        playerRenderer.Origin = new(playerImage.Width / 2f, playerImage.Height / 2f);
        player.AddComponent(playerRenderer);

        // Scale the player transform down because the placeholder sprite is MAHOOSIVE.
        player.Transform.CurrentScale = new Vector2(0.2f, 0.2f);

        // Whack a camera on the player so we can see it in action.
        Camera playerCam = new();
        player.AddComponent(playerCam);

        // Add the "player" to the scene.
        _activeScene.AddEntity(player);

        _activeScene.MainCamera = playerCam;

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

        // The active scene calls the draw method for any Entities that need it.
        _activeScene.Draw(_spriteBatch);

        base.Draw(gameTime);

    }
}