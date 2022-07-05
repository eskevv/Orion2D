using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Orion2D;
public class CoreGame : Game {

   private GraphicsDeviceManager _graphics;
   private SpriteBatch _spriteBatch;

   private Input _input;
   private Shapes _shapes;
   private double _lastUpdate;
   private double _totalTime;
   private double _fps;

   public static EntityRegistry Registry { get; private set; }
   public static Dictionary<string, Texture2D> Textures { get; private set; }
   public static Dictionary<string, SpriteFont> Fonts { get; private set; }
   public static Dictionary<string, SoundEffect> Sounds { get; private set; }

   private MovementSystem _movementSystem;
   private RenderSystem _renderSystem;
   private ScriptSystem _scriptSystem;
   private PhysicsSystem _physicsSystem;
   private AnimationSystem _animaitonSystem;

   public static Color ClearColr => new Color(24, 24, 24);
   public static int ScreenWidth => 1600;
   public static int ScreenHeight => 900;
   public static int TargetFPS => 120;
   public static bool IsVSync => false;
   public static bool IsFullScreen => false;

   public CoreGame()
   {
      _graphics = new GraphicsDeviceManager(this);
      _input = new Input();
      _shapes = new Shapes();


      Registry = new EntityRegistry();
      Textures = new Dictionary<string, Texture2D>();
      Fonts = new Dictionary<string, SpriteFont>();
      Sounds = new Dictionary<string, SoundEffect>();
   }

   // __Definitions__

   protected override void Initialize()
   {
      _spriteBatch = new SpriteBatch(GraphicsDevice);

      _graphics.PreferredBackBufferWidth = ScreenWidth;
      _graphics.PreferredBackBufferHeight = ScreenHeight;
      _graphics.SynchronizeWithVerticalRetrace = IsVSync;
      _graphics.IsFullScreen = IsFullScreen;
      _graphics.ApplyChanges();

      _shapes.Initialize(this);

      Content.RootDirectory = "content";
      IsMouseVisible = true;
      TargetElapsedTime = System.TimeSpan.FromSeconds(1d / TargetFPS);
      SoundEffect.MasterVolume = 0.01f;

      // -- -- Objects / ECS -- --

      Registry.RegisterComponent<Transform>();
      Registry.RegisterComponent<RigidBody>();
      Registry.RegisterComponent<SpriteRenderer>();
      Registry.RegisterComponent<Script>();
      Registry.RegisterComponent<Collider>();
      Registry.RegisterComponent<Animator>();

      _movementSystem = Registry.RegisterSystem<MovementSystem>();
      _renderSystem = Registry.RegisterSystem<RenderSystem>();
      _scriptSystem = Registry.RegisterSystem<ScriptSystem>();
      _physicsSystem = Registry.RegisterSystem<PhysicsSystem>();
      _animaitonSystem = Registry.RegisterSystem<AnimationSystem>();

      var movement_signature = new BitArray(ComponentManager.MaxComponents);
      movement_signature.SetBits(Registry.GetComponentType<Transform>());
      movement_signature.SetBits(Registry.GetComponentType<RigidBody>());
      Registry.SetSystemSignature<MovementSystem>(movement_signature);

      var render_signature = new BitArray(ComponentManager.MaxComponents);
      render_signature.SetBits(Registry.GetComponentType<SpriteRenderer>());
      render_signature.SetBits(Registry.GetComponentType<Transform>());
      Registry.SetSystemSignature<RenderSystem>(render_signature);
      
      var animation_signature = new BitArray(ComponentManager.MaxComponents);
      animation_signature.SetBits(Registry.GetComponentType<SpriteRenderer>());
      animation_signature.SetBits(Registry.GetComponentType<Transform>());
      animation_signature.SetBits(Registry.GetComponentType<Animator>());
      Registry.SetSystemSignature<AnimationSystem>(animation_signature);

      var script_signature = new BitArray(ComponentManager.MaxComponents);
      script_signature.SetBits(Registry.GetComponentType<Script>());
      Registry.SetSystemSignature<ScriptSystem>(script_signature);

      var physics_signature = new BitArray(ComponentManager.MaxComponents);
      physics_signature.SetBits(Registry.GetComponentType<Collider>());
      physics_signature.SetBits(Registry.GetComponentType<Transform>());
      physics_signature.SetBits(Registry.GetComponentType<RigidBody>());
      Registry.SetSystemSignature<PhysicsSystem>(physics_signature);

      // --- --- --- --- --- --- --- --- --- --- --- ---

      base.Initialize();
   }

   protected override void LoadContent()
   {
      Textures["space-drone"] = Content.Load<Texture2D>("space-drone");
      Textures["space-bg"] = Content.Load<Texture2D>("space-bg");
      Textures["space-bullet1"] = Content.Load<Texture2D>("space-bullet1");
      Textures["explosion"] = Content.Load<Texture2D>("explosion");
      Textures["enemy-space-02"] = Content.Load<Texture2D>("enemy-space-02");
      Textures["enemy-space-03"] = Content.Load<Texture2D>("enemy-space-03");
      Textures["enemy-space-04"] = Content.Load<Texture2D>("enemy-space-04");
      Sounds["explosion-hit"] = Content.Load<SoundEffect>("explosion-hit");
      Fonts["fps-font"] = Content.Load<SpriteFont>("fps-font");

      Factory.CreateSpaceDrone(playerScript: true, "space-drone");
      Factory.CreateSpaceBackground(new Vector2(0f, 0f));
      Factory.CreateSpaceBackground(new Vector2(ScreenWidth, 0f));

      Texture2D[] enemyTwoAnimation = new[] { Textures["enemy-space-02"], Textures["enemy-space-03"], Textures["enemy-space-04"] };

      for (int r = 0; r < 130; r++)
      {
         ushort enemyDrone = Factory.CreateSpaceDrone(playerScript: false, "enemy-space-02", true);
      }

      Factory.CreateSpawner();
   }

   protected override void Update(GameTime gameTime)
   {
      _input.Update();

      if (Input.KeyPressed(Key.Escape))
      {
         Exit();
      }

      float deltaTime = (float)(gameTime.TotalGameTime.TotalSeconds - _lastUpdate);
      _fps = 1d / (deltaTime);

      Registry.UpdateRegistry();
      _scriptSystem.Update(deltaTime);
      _movementSystem.Update(deltaTime);
      _physicsSystem.Update(deltaTime);
      _animaitonSystem.Update(deltaTime);

      base.Update(gameTime);
   }

   protected override void Draw(GameTime gameTime)
   {
      GraphicsDevice.Clear(ClearColr);

      _renderSystem.Render(_spriteBatch);
      
      Shapes.DrawFillRect(new Vector2(0f, 0f), 230, 140, new Color(30, 30, 30, 255));

      _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      _spriteBatch.DrawString(Fonts["fps-font"], $"{(int)_fps} FPS", new Vector2(10f, 10f), Color.White);
      _spriteBatch.DrawString(Fonts["fps-font"], $"# of Entities: {Registry.TotalEntities.ToString()}", new Vector2(10f, 50f), Color.BlueViolet);
      _spriteBatch.DrawString(Fonts["fps-font"], $"Time: {System.Math.Round(gameTime.TotalGameTime.TotalSeconds, 2).ToString()}", new Vector2(10f, 90f), Color.CadetBlue);
      _spriteBatch.End();

      _lastUpdate += gameTime.ElapsedGameTime.TotalSeconds;

      base.Draw(gameTime);
   }
}
