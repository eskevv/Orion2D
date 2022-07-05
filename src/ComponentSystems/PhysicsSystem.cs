using System.Linq;

namespace Orion2D;
public class PhysicsSystem : ComponentSystem {

   // __Definitions

   public void Update(float deltaTime)
   {
      UpdateColliders();

      var entities = Entities.ToList();
      for (int x = 0; x < Entities.Count; x++)
      {
         ushort firstEntity = entities[x];

         Collider a = CoreGame.Registry.GetComponent<Collider>(firstEntity);

         for (int y = x + 1; y < Entities.Count; y++)
         {
            ushort secondEntity = entities[y];

            Collider b = CoreGame.Registry.GetComponent<Collider>(secondEntity);

            bool collision_happened = CollisionAABB(a, b);

            if (collision_happened)
            {
               NotifyScripts(firstEntity, secondEntity, a, b);
            }
         }
      }
   }

   public bool CollisionAABB(Collider a, Collider b)
   {
      bool collide_left = a.X <= b.X + b.Width;
      bool collide_right = a.X + a.Width >= b.X;
      bool collide_top = a.Y <= b.Y + b.Height;
      bool collide_bottom = a.Y + a.Height >= b.Y;

      return collide_left && collide_right && collide_top && collide_bottom;
   }

   private void UpdateColliders()
   {
      foreach (var item in Entities)
      {
         Transform transform = CoreGame.Registry.GetComponent<Transform>(item);
         Collider collider = CoreGame.Registry.GetComponent<Collider>(item);
         collider.X = transform.Position.X;
         collider.Y = transform.Position.Y;
      }
   }

   private void NotifyScripts(ushort firstEntity, ushort secondEntity, Collider a, Collider b)
   {
      if (CoreGame.Registry.HasComponentType<Script>(firstEntity))
      {
         var script = CoreGame.Registry.GetComponent<Script>(firstEntity);
         script.OnCollision(b);
      }
      if (CoreGame.Registry.HasComponentType<Script>(secondEntity))
      {
         var script = CoreGame.Registry.GetComponent<Script>(secondEntity);
         script.OnCollision(a);
      }
   }
}
