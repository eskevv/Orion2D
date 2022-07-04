using System;
using System.Linq;

namespace Orion2D;
public class PhysicsSystem : ComponentSystem {
   // __Methods__

   public static event Action<ushort, ushort> CollidingObjects;

   public void Update(float deltaTime)
   {
      RecentUpdates();
      foreach (var item in Entities)
      {
         var transform = CoreGame.Registry.GetComponent<Transform>(item);
         var collider = CoreGame.Registry.GetComponent<Collider>(item);
         collider.X = transform.Position.X;
         collider.Y = transform.Position.Y;
      }

      var entities = Entities.ToList();
      for (int x = 0; x < Entities.Count; x++)
      {
         ushort item = entities[x];
         var collider = CoreGame.Registry.GetComponent<Collider>(item);

         for (int y = x; y < Entities.Count; y++)
         {
            if (x == y) continue;
            ushort item2 = entities[y];
            var collider2 = CoreGame.Registry.GetComponent<Collider>(item2);
            bool collision_happened = TestCollision(collider, collider2);

            if (collision_happened)
            {
               if (CoreGame.Registry.HasComponentType<Script>(item))
               {
                  var script = CoreGame.Registry.GetComponent<Script>(item);
                  script.OnCollision(collider2);
               }
               if (CoreGame.Registry.HasComponentType<Script>(item2))
               {
                  var script = CoreGame.Registry.GetComponent<Script>(item2);
                  script.OnCollision(collider);
               }
               // CollidingObjects?.Invoke(item, item2);
            }
         }
      }
   }

   public bool TestCollision(Collider a, Collider b)
   {
      bool collide_left = a.X <= b.X + b.Width;
      bool collide_right = a.X + a.Width >= b.X;
      bool collide_top = a.Y <= b.Y + b.Height;
      bool collide_bottom = a.Y + a.Height >= b.Y;

      return collide_left && collide_right && collide_top && collide_bottom;
   }
}
