using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CorleyEngine.Components;
using System;
using System.IO;

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

        Log.Info("Starting Engine...");

        // Get path to source directory.
        string runtimeDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectRoot = Path.GetFullPath(Path.Combine(runtimeDir, @"..\..\..\"));

        // Get path to folder where compiled xnb files live.
        string compiledAssetsPath = Path.Combine(projectRoot, "Content", "bin", "DesktopGL");

        ProjectManager.LoadProject(@"G:\Corley Engine\SampleProject\Sample Project.corleyproject");

        AssetManager.Initialise(ProjectManager.CurrentProject.AbsoluteAssetPath, GraphicsDevice);

        // Make a new scene.
        _activeScene = new Scene();

        base.Initialize();

    }

    protected override void LoadContent() {

        // Since the game engine part uses data streaming to load standard media files, this section is exclusively
        // for engine-only content that has been compiled into .xnb files. The active scene will handle everything
        // else.

        Log.Info("Loading Binaries...");

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SceneManager.LoadScene("TestLevel");

        // SceneManager.CreateScene("TestLevel");
        // SceneManager.LoadScene("TestLevel");
        // SceneData blueprint = SceneManager.ActiveScene.Data;


        // // Temporary background image for camera testing.
        // Texture2D bgImage = AssetManager.Get<Texture2D>("tempbg.png");
        // Entity background = Entity.CreateStageEntity("Background", new());
        // background.AddComponent(new SpriteRenderer("tempbg.png"));
        // _activeScene.AddEntity(background);

        // // Create a moveable "player" for testing.
        // Entity player = Entity.CreateStageEntity("Player", new(100, 100));
        // player.AddComponent(new CharacterController());
        // player.AddComponent(new PlayerInput());

        // // Load in a placeholder player sprite and set its pivot point to
        // // the centre of the sprite.
        // //Texture2D playerImage = AssetManager.Get<Texture2D>("PlaceholderMan.png");
        // SpriteRenderer playerRenderer = new("PlaceholderMan.png");
        // //playerRenderer.Origin = new(playerImage.Width / 2f, playerImage.Height / 2f);
        // player.AddComponent(playerRenderer);

        // // Scale the player transform down because the placeholder sprite is MAHOOSIVE.
        // player.Transform.Scale = new Vector2(0.2f, 0.2f);

        // // Whack a camera on the player so we can see it in action.
        // Camera playerCam = new();
        // player.AddComponent(playerCam);

        // // Add the "player" to the scene.
        // _activeScene.AddEntity(player);

        // _activeScene.MainCamera = playerCam;

        // blueprint.Entities.Add(background);
        // blueprint.Entities.Add(player);
        // SceneManager.SaveScene();

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