namespace Orion2D;
public class MovementSystem : ComponentSystem {
   // __Methods__

   public void Update(float deltaTime)
   {
      RecentUpdates();

      foreach (var item in Entities)
      {
         var transform = Coordinator.GetComponent<Transform>(item);
         var rigid_body = Coordinator.GetComponent<RigidBody>(item);

         transform.Position += rigid_body.Velocty * deltaTime * 30f;
      }
   }
}
