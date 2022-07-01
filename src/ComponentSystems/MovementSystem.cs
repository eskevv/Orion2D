namespace Orion2D;
public class MovementSystem : ComponentSystem {
	// __Methods__

	public void Update(float deltaTime)
	{
		RecentUpdates();

		foreach (var item in Entities)
		{
			var transform = CoreGame.Registry.GetComponent<Transform>(item);
			var rigid_body = CoreGame.Registry.GetComponent<RigidBody>(item);

			transform.Position += rigid_body.Velocty * deltaTime;
		}
	}
}
