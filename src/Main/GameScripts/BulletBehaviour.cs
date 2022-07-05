using Microsoft.Xna.Framework;
using System;

namespace Orion2D;

public class BulletBehaviour : Script {

   private RigidBody _rb;
   private Transform _tr;
   private float _speed;
   private bool _alive;
   private int _pierce;

   private Random _r;

   public override void Awake()
   {
      _rb = GetComponent<RigidBody>();
      _tr = GetComponent<Transform>();
      _r = new Random();
   }

   public override void Start()
   {
      _speed = 30f;
      _alive = true;
      _pierce = 7;
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
      string tag = CoreGame.Registry.GetTag(c.Entity);
      if (c.Entity != 0 && tag != Tags.Projectile)
      {
         int rC = _r.Next(255);
         int gC = _r.Next(170);
         int bC = _r.Next(200);
         var colr = new Color(rC, gC, bC, 255);

         for (int t = 0; t < _r.Next(14) + 5; t++)
         {
            Factory.CreateExplosion(_tr.Position, colr);
         }

         CoreGame.Registry.DestroyEntity(c.Entity);

         if (_pierce-- == 0)
         {
            CoreGame.Registry.DestroyEntity(Entity);
         }
      }
   }
}
