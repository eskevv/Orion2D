using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Orion2D;
public static class Factory {

   private static Random _randomizer = new Random();

   private static EntityRegistry _registry => CoreGame.Registry;

   // __Definitions__

   public static EntityHandle CreateSpaceDrone(bool playerScript, string image, bool animated = false)
   {
      EntityHandle space_drone = _registry.CreateEntity();

      var r = new Random();
      var position = new Vector2(r.Next(1500), r.Next(800));
      var spriteRenderer = new SpriteRenderer(CoreGame.Textures[image]);
      spriteRenderer.ZIndex = 2;
      Collider a = new Collider {
         X = position.X,
         Y = position.Y,
         Width = spriteRenderer.Sprite.Width,
         Height = spriteRenderer.Sprite.Height,
      };

      if (animated)
      {
         Texture2D[] enemyTwoAnimation = new[] { CoreGame.Textures["enemy-space-02"], CoreGame.Textures["enemy-space-03"], CoreGame.Textures["enemy-space-04"] };
         var animationOne = new AnimationClip(enemyTwoAnimation, 0.8f);
         var animator = new Animator();
         animator.AddAnimation(animationOne, "Parol");
         space_drone.AddComponent<Animator>(animator);
      }

      space_drone.AddComponent<Transform>(new Transform(position));
      space_drone.AddComponent<SpriteRenderer>(spriteRenderer);
      space_drone.AddComponent<Collider>(a);
      space_drone.AddComponent<RigidBody>(new RigidBody());

      if (playerScript)
      {
         space_drone.AddComponent<Script>(new SpaceDroneController());
         space_drone.GetComponent<Script>().Awake();
      }

      return space_drone;
   }

   public static EntityHandle CreateSpaceBackground(Vector2 position)
   {
      EntityHandle background = _registry.CreateEntity();

      var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bg"]);
      spriteRenderer.ZIndex = 1;
      spriteRenderer.Maximized = true;

      background.AddComponent<Transform>(new Transform(position));
      background.AddComponent<SpriteRenderer>(spriteRenderer);
      background.AddComponent<Script>(new ParallaxScrolling());

      background.GetComponent<Script>().Awake();

      return background;
   }

   public static EntityHandle CreateSpawner()
   {
      EntityHandle spawner = _registry.CreateEntity();

      spawner.AddComponent<Script>(new DroneSpawner());

      spawner.GetComponent<Script>().Awake();

      return spawner;
   }

   public static EntityHandle CreateExplosion(Vector2 position, Color color)
   {
      EntityHandle explosion = _registry.CreateEntity();

      var renderer = new SpriteRenderer(CoreGame.Textures["explosion"]);

      renderer.Color = color;
      renderer.ZIndex = 4;
      renderer.Additive = true;
      renderer.Origin = new Vector2(256, 256);
      renderer.Rotation = MathHelper.ToRadians(_randomizer.Next(360));

      position += new Vector2(24f, 24f);

      explosion.AddComponent<Transform>(new Transform(position));
      explosion.AddComponent<SpriteRenderer>(renderer);
      explosion.AddComponent<Script>(new Explosions());

      explosion.GetComponent<Script>().Awake();
      return explosion;
   }

   public static EntityHandle CreateSpaceBullet(Vector2 position, Vector2 direction)
   {
      EntityHandle bullet = _registry.CreateEntity();

      var sp = new SpriteRenderer(CoreGame.Textures["space-bullet1"]) {
         ZIndex = 3,
      };
      var co = new Collider {
         X = position.X,
         Y = position.Y,
         Width = sp.Sprite.Width,
         Height = sp.Sprite.Height,
      };
      var rb = new RigidBody() {
         Velocty = direction,
      };

      bullet.AddComponent<RigidBody>(rb);
      bullet.AddComponent<Collider>(co);
      bullet.AddComponent<Transform>(new Transform(position));
      bullet.AddComponent<SpriteRenderer>(sp);
      bullet.AddComponent<Script>(new BulletBehaviour());
      bullet.AssignTag(Tags.Projectile);
      bullet.GetComponent<Script>().Awake();

      return bullet;
   }
}
