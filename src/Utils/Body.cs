using Microsoft.Xna.Framework;

namespace Orion2D;
public class Body {
   // __Fields__

   public Vector2 Position { get; set; }
   public Vector2 Acceleration { get; set; }
   public Vector2 Velocity { get; set; }

   public Vector2 SumForces { get; private set; }
   public Shape Shape { get; private set; }
   public float Mass { get; private set; }
   public float InvMass { get; private set; }
   public bool Active { get; set; }

   public Body(Shape shape, float x, float y, float mass)
   {
      Shape = shape;
      Position = new Vector2(x, y);
      Mass = mass;
      InvMass = mass != 0f ? 1f / mass : 0f;
   }

   // __Methods__

   public void AddForce(Vector2 force)
   {
      SumForces += force;
   }

   public void ClearForces()
   {
      SumForces = Vector2.Zero;
   }

   public void Integrate(float dt)
   {
      if (Active)
      {
         Acceleration = SumForces * InvMass;
         Velocity += Acceleration * dt;
         Position += Velocity * dt;
      }

      Active = true;

      ClearForces();
   }
}