using Microsoft.Xna.Framework;
using System;

namespace Orion2D;
public static class EntityCreator {
	// __Fields__

	private static EntityRegistry _registry => CoreGame.Registry;

	public static ushort CreateSpaceDrone()
	{
		ushort space_drone = _registry.CreateEntity();

		var r = new Random();
		var position = new Vector2(r.Next(300), r.Next(500));
		_registry.AddComponent<Transform>(space_drone, new Transform(position));

		var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-drone"]);
		spriteRenderer.ZIndex = 2;

		_registry.AddComponent<SpriteRenderer>(space_drone, spriteRenderer);
		_registry.AddComponent<RigidBody>(space_drone, new RigidBody(new Vector2(0f, 0f), new Vector2(0f, 0f)));
		_registry.AddComponent<Script>(space_drone, new SpaceDroneController());

		_registry.GetComponent<Script>(space_drone).Awake();
		return space_drone;
	}

	public static ushort CreateSpaceBackground()
	{
		ushort background = _registry.CreateEntity();

		var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bg"], fitScreen: true);
		spriteRenderer.ZIndex = 1;

		_registry.AddComponent<Transform>(background, new Transform(new Vector2(0f, 0f)));
		_registry.AddComponent<SpriteRenderer>(background, spriteRenderer);

		return background;
	}
}

public class SpaceDroneController : Script {
	// __Fields__

	private RigidBody _rb;

	private float _directionX;
	private float _directionY;
	private float _speed = 15f;

	// __Methods__

	public override void Awake()
	{
		_rb = GetComponent<RigidBody>();
	}

	public override void Update(float deltaTime)
	{
		_directionX = Input.RawHorizontal;
		_directionY = Input.RawVertical;

		Vector2 direction = new Vector2(_directionX, _directionY);
		direction = direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction);

		_rb.Velocty = direction * _speed * deltaTime;
	}
}