using Microsoft.Xna.Framework;
using System;

namespace Orion2D;
public static class Factory {
   // __Fields__

   private static Random _randomizer = new Random();

   private static EntityRegistry _registry => CoreGame.Registry;

   public static ushort CreateSpaceDrone(bool playerScript)
   {
      ushort space_drone = _registry.CreateEntity();

      var r = new Random();
      var position = new Vector2(r.Next(1500), r.Next(800));
      var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-drone"]);
      spriteRenderer.ZIndex = 2;
      Collider a = new Collider {
         X = position.X,
         Y = position.Y,
         Width = spriteRenderer.Sprite.Width,
         Height = spriteRenderer.Sprite.Height,
      };

      _registry.AddComponent<Transform>(space_drone, new Transform(position));
      _registry.AddComponent<SpriteRenderer>(space_drone, spriteRenderer);
      _registry.AddComponent<Collider>(space_drone, a);
      _registry.AddComponent<RigidBody>(space_drone, new RigidBody());

      if (playerScript)
      {
         _registry.AddComponent<Script>(space_drone, new SpaceDroneController());
         _registry.GetComponent<Script>(space_drone).Awake();
      }

      return space_drone;
   }

   public static ushort CreateSpaceBackground(Vector2 position)
   {
      ushort background = _registry.CreateEntity();

      var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bg"]);
      spriteRenderer.ZIndex = 1;
      spriteRenderer.Maximized = true;

      _registry.AddComponent<Transform>(background, new Transform(position));
      _registry.AddComponent<SpriteRenderer>(background, spriteRenderer);
      _registry.AddComponent<Script>(background, new ParallaxScrolling());

      _registry.GetComponent<Script>(background).Awake();

      return background;
   }

   public static ushort CreateSpaceBullet(Vector2 position, Vector2 direction)
   {
      ushort bullet = _registry.CreateEntity();

      var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bullet1"]);
      spriteRenderer.ZIndex = 3;

      Collider a = new Collider {
         X = position.X,
         Y = position.Y,
         Width = spriteRenderer.Sprite.Width,
         Height = spriteRenderer.Sprite.Height,
      };

      RigidBody rb = new RigidBody();
      rb.Velocty = direction;

      _registry.AddComponent<RigidBody>(bullet, rb);
      _registry.AddComponent<Collider>(bullet, a);
      _registry.AddComponent<Transform>(bullet, new Transform(position));
      _registry.AddComponent<SpriteRenderer>(bullet, spriteRenderer);
      _registry.AddComponent<Script>(bullet, new BulletBehaviour());

      _registry.AssignTag(bullet, "projectile");

      _registry.GetComponent<Script>(bullet).Awake();
      return bullet;
   }

   public static ushort CreateSpawner()
   {
      ushort spawner = _registry.CreateEntity();

      _registry.AddComponent<Script>(spawner, new DroneSpawner());

      _registry.GetComponent<Script>(spawner).Awake();

      return spawner;
   }

   public static ushort CreateExplosion(Vector2 position, Color color)
   {
      ushort explosion = _registry.CreateEntity();

      var renderer = new SpriteRenderer(CoreGame.Textures["explosion"]);

      renderer.Color = color;
      renderer.ZIndex = 4;
      renderer.Additive = true;
      renderer.Origin = new Vector2(256, 256);
      renderer.Rotation = MathHelper.ToRadians(_randomizer.Next(360));

      position += new Vector2(24f, 24f);

      _registry.AddComponent<Transform>(explosion, new Transform(position));
      _registry.AddComponent<SpriteRenderer>(explosion, renderer);
      _registry.AddComponent<Script>(explosion, new Explosions());

      _registry.GetComponent<Script>(explosion).Awake();
      return explosion;
   }
}

