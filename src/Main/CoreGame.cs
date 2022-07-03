using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Orion2D;
public class CoreGame : Game {
	// __Fields__

	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private Input _input;
	private System.TimeSpan _lastUpdate;
	private float _timeScale = 30f; // delta scale relative to per second calculations
	private float _fps;

	public static EntityRegistry Registry { get; private set; }
	public static Dictionary<string, Texture2D> Textures { get; private set; }
	public static Dictionary<string, SpriteFont> Fonts { get; private set; }

	private MovementSystem _movementSystem;
	private RenderSystem _renderSystem;
	private ScriptSystem _scriptSystem;
	private PhysicsSystem _physicsSystem;

	public static Color ClearColr => new Color(24, 24, 24);
	public static int ScreenWidth => 1600;
	public static int ScreenHeight => 900;
	public static int TargetFPS => 120;
	public static bool IsVSync => false;
	public static bool IsFullScreen => false;

	// GameObjects

	ushort _spaceDrone;
	ushort _background;

	public CoreGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		_input = new Input();

		Registry = new EntityRegistry();
		Textures = new Dictionary<string, Texture2D>();
		Fonts = new Dictionary<string, SpriteFont>();
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
		Registry.RegisterComponent<Script>();
		Registry.RegisterComponent<Collider>();

		_movementSystem = Registry.RegisterSystem<MovementSystem>();
		_renderSystem = Registry.RegisterSystem<RenderSystem>();
		_scriptSystem = Registry.RegisterSystem<ScriptSystem>();
		_physicsSystem = Registry.RegisterSystem<PhysicsSystem>();

		var movement_signature = new BitArray(ComponentManager.MaxComponents);
		movement_signature.SetBits(Registry.GetComponentType<Transform>());
		movement_signature.SetBits(Registry.GetComponentType<RigidBody>());
		Registry.SetSystemSignature<MovementSystem>(movement_signature);

		var render_signature = new BitArray(ComponentManager.MaxComponents);
		render_signature.SetBits(Registry.GetComponentType<SpriteRenderer>());
		Registry.SetSystemSignature<RenderSystem>(render_signature);

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
		Fonts["fps-font"] = Content.Load<SpriteFont>("fps-font");

		_spaceDrone = EntityCreator.CreateSpaceDrone(playerScript: true);
		ushort _spaceDrone2 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone3 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone4 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone5 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone6 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone7 = EntityCreator.CreateSpaceDrone(playerScript: false);
		ushort _spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);
		_spaceDrone8 = EntityCreator.CreateSpaceDrone(playerScript: false);

		_background = EntityCreator.CreateSpaceBackground(new Vector2(0f, 0f));
		ushort _background2 = EntityCreator.CreateSpaceBackground(new Vector2(ScreenWidth, 0f));
	}

	protected override void Update(GameTime gameTime)
	{
		_input.Update();
		if (Input.KeyPressed(Key.Escape))
		{
			Exit();
		}

		float deltaTime = (float)(gameTime.TotalGameTime - _lastUpdate).TotalSeconds * _timeScale;
		_fps = 1f / (deltaTime / _timeScale);
		_lastUpdate = gameTime.TotalGameTime;

		Registry.UpdateRegistry();

		_scriptSystem.Update(deltaTime);
		_movementSystem.Update(deltaTime);
		_physicsSystem.Update(deltaTime);

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(ClearColr);

		_renderSystem.Render(_spriteBatch);

		_spriteBatch.Begin();
		_spriteBatch.DrawString(Fonts["fps-font"], $"{(int)_fps} FPS", new Vector2(10f, 10f), Color.White);
		_spriteBatch.End();

		base.Draw(gameTime);
	}
}
