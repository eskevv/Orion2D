using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Orion2D;
public class SpaceDroneController : Script {
   // __Fields__

   private RigidBody _rb;
   private Transform _tr;
   private SpriteRenderer _sp;

   private float _directionX;
   private float _directionY;
   private float _speed = 20f;
   private bool _canShoot;
   private float _fireRate = 20f;
   private float _shootTimer;

   // __Methods__

   public override void Awake()
   {
      _rb = GetComponent<RigidBody>();
      _tr = GetComponent<Transform>();
      _sp = GetComponent<SpriteRenderer>();
   }

   public override void Update(float deltaTime)
   {
      _shootTimer += deltaTime;
      _canShoot = IsAbleToShoot();
      _directionX = Input.RawHorizontal;
      _directionY = Input.RawVertical;

      Vector2 move_direction = new Vector2(_directionX, _directionY);
      move_direction = move_direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(move_direction);
      _rb.Velocty = move_direction * _speed;

      if (Input.MouseHeld(MouseButton.LeftButton) && _canShoot)
      {
         ShootBullet();
         _canShoot = false;
      }
   }

   private void ShootBullet()
   {
      Vector2 bullet_origin = _tr.Position + new Vector2(_sp.Sprite.Width / 2, 0f) - new Vector2(8f, 0f);
      Vector2 bullet_dir = Vector2.Normalize(Input.MousePosition - bullet_origin);
      Factory.CreateSpaceBullet(bullet_origin, bullet_dir);
   }

   private bool IsAbleToShoot()
   {
      if (_shootTimer >= 1f / _fireRate)
      {
         _shootTimer = 0f;
         return true;
      }
      return _canShoot;
   }
}

public class ParallaxScrolling : Script {
   // __Fields__

   private SpriteRenderer _imageOne;
   private Transform _transform;
   private float _speed = 0.4f;

   public override void Awake()
   {
      _imageOne = GetComponent<SpriteRenderer>();
      _transform = GetComponent<Transform>();
   }

   public override void Update(float deltaTime)
   {
      _transform.Position -= new Vector2(_speed, 0f);
      if (_transform.Position.X <= -CoreGame.ScreenWidth)
      {
         float diff = -CoreGame.ScreenWidth - _transform.Position.X;
         _transform.Position = new Vector2(CoreGame.ScreenWidth - diff, 0);
      }
   }
}

public class BulletBehaviour : Script {
   // __Fields

   private RigidBody _rb;
   private Transform _tr;
   private float _speed = 30f;
   private bool _alive = true;

   private int _pierce = 2;

   private Random _r = new Random();

   public override void Awake()
   {
      _rb = GetComponent<RigidBody>();
      _tr = GetComponent<Transform>();
      _rb.Velocty *= _speed;
   }

   public override void Update(float deltaTime)
   {
      _alive = _tr.Position.X > CoreGame.ScreenWidth || _tr.Position.X < 0 ? false : _alive;
      _alive = _tr.Position.Y > CoreGame.ScreenHeight || _tr.Position.Y < 0 ? false : _alive;

      if (!_alive)
      {
         CoreGame.Registry.DestroyEntity(Entity);
      }
   }

   public override void OnCollision(Collider c)
   {
      string tag = CoreGame.Registry.RetrieveTag(c.Entity);
      if (c.Entity != 0 && tag != "projectile")
      {
         int rC = _r.Next(255);
         int gC = _r.Next(170);
         int bC = _r.Next(200);
         var colr = new Color(rC, gC, bC, 255);

         for (int t = 0; t < _r.Next(16) + 2; t++)
         {
            Factory.CreateExplosion(_tr.Position, colr);
         }

         CoreGame.Registry.DestroyEntity(c.Entity);

         if (--_pierce == 0)
         {
            CoreGame.Registry.DestroyEntity(Entity);
         }
      }
   }
}

public class DroneSpawner : Script {

   private float _spawnTimer;
   private float _spawnInterval;

   public override void Awake()
   {
      _spawnInterval = 0f;

   }

   public override void Update(float deltaTime)
   {
      _spawnTimer += deltaTime;

      if (_spawnTimer >= _spawnInterval && _spawnInterval != 0)
      {
         _spawnTimer = 0;
         Factory.CreateSpaceDrone(false);
      }
   }
}

public class Explosions : Script {

   private SpriteRenderer _sp;

   private Random _r = new Random();

   private float _lifeTime;
   private float _maxLifeTime;

   private static SoundEffect _hitFx = CoreGame.Sounds["explosion-hit"];

   public override void Awake()
   {
      _sp = GetComponent<SpriteRenderer>();
      _maxLifeTime = 1.4f;
      _hitFx.Play();
   }

   public override void Update(float deltaTime)
   {
      _lifeTime += deltaTime;
      float relAge = _lifeTime / _maxLifeTime;

      _sp.Scale = new Vector2((1f - relAge) + 0.1f, (1f - relAge) + 0.1f);

      byte alphaC = (byte)(255 * ((1f - relAge) + 0.1f));
      _sp.Color = new Color(_sp.Color.R, _sp.Color.G, _sp.Color.B, alphaC);

      if (_lifeTime >= _maxLifeTime)
      {
         CoreGame.Registry.DestroyEntity(Entity);
      }
   }
}