using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Orion2D;
public class CoreGame : Game
{
    // __Fields__

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    public static EntityRegistry Registry;
    private MovementSystem _movementSystem;
    private RenderSystem _renderSystem;

    public static Color ClearColr = new Color(24, 24, 24);
    public static int ScreenWidth => 1920;
    public static int ScreenHeight => 1080;
    public static int TargetFPS => 120;
    public static bool IsVSync => false;
    public static bool IsFullScreen => false;

    // GameObjects
    
    public static Dictionary<string, Texture2D> _textures_;
    ushort _space_drone;

    public CoreGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Registry = new EntityRegistry();
        _textures_ = new Dictionary<string, Texture2D>();
    }

    // __Methods__

    protected override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _graphics.PreferredBackBufferWidth = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;
        _graphics.SynchronizeWithVerticalRetrace = IsVSync;
        _graphics.IsFullScreen = IsFullScreen;
        _graphics.ApplyChanges();

        Content.RootDirectory = "content";
        IsMouseVisible = true;
        TargetElapsedTime = System.TimeSpan.FromSeconds(1d / TargetFPS);
         
        // -- -- Objects / ECS -- --

        Registry.RegisterComponent<Transform>();
        Registry.RegisterComponent<RigidBody>();
        Registry.RegisterComponent<SpriteRenderer>();

        _movementSystem = Registry.RegisterSystem<MovementSystem>();
        _renderSystem = Registry.RegisterSystem<RenderSystem>();
        
        var movement_signature = new BitArray(ComponentManager.MaxComponents);
        movement_signature.SetBits(Registry.GetComponentType<Transform>());
        movement_signature.SetBits(Registry.GetComponentType<RigidBody>());
        Registry.SetSystemSignature<MovementSystem>(movement_signature);

        var render_signature = new BitArray(ComponentManager.MaxComponents);
        render_signature.SetBits(Registry.GetComponentType<SpriteRenderer>());
        Registry.SetSystemSignature<RenderSystem>(render_signature);

        // --- --- --- --- --- --- --- --- --- --- --- ---

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _textures_["space-drone"] = Content.Load<Texture2D>("space-drone");
        _space_drone = EntityCreator.CreateSpaceDrone(new Vector2(500f, 20f));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
        }

        _movementSystem.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearColr);

        _spriteBatch.Begin();
        _renderSystem.Render(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
