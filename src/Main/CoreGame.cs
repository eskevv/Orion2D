﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Orion2D;
public class CoreGame : Game
{
   // __Fields__

   private GraphicsDeviceManager _graphics;
   private SpriteBatch _spriteBatch;

   private Input _input;

   public static EntityRegistry Registry { get; private set; }
   public static Dictionary<string, Texture2D> Textures { get; private set; }

   private MovementSystem _movementSystem;
   private RenderSystem _renderSystem;
   private ScriptSystem _scriptSystem;

   public static Color ClearColr = new Color(24, 24, 24);
   public static int ScreenWidth => 1600;
   public static int ScreenHeight => 1000;
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

      _movementSystem = Registry.RegisterSystem<MovementSystem>();
      _renderSystem = Registry.RegisterSystem<RenderSystem>();
      _scriptSystem = Registry.RegisterSystem<ScriptSystem>();

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

      // --- --- --- --- --- --- --- --- --- --- --- ---

      base.Initialize();
   }

   protected override void LoadContent()
   {
      Textures["space-drone"] = Content.Load<Texture2D>("space-drone");
      Textures["space-bg"] = Content.Load<Texture2D>("space-bg");

      _spaceDrone = EntityCreator.CreateSpaceDrone();
      _background = EntityCreator.CreateSpaceBackground();
   }

   protected override void Update(GameTime gameTime)
   {
      _input.Update();
      if (Input.KeyPressed(Key.Escape))
      {
         Exit();
      }

      _scriptSystem.Update();
      _movementSystem.Update();

      base.Update(gameTime);
   }

   protected override void Draw(GameTime gameTime)
   {
      GraphicsDevice.Clear(ClearColr);

      _renderSystem.Render(_spriteBatch);

      base.Draw(gameTime);
   }
}
