using Microsoft.Xna.Framework;
using System;

namespace Orion2D;
public static class EntityCreator {
	// __Fields__

	private static EntityRegistry _registry => CoreGame.Registry;

	public static ushort CreateSpaceDrone(bool playerScript)
	{
		ushort space_drone = _registry.CreateEntity();

		var r = new Random();
		var position = new Vector2(r.Next(900), r.Next(900));
		_registry.AddComponent<Transform>(space_drone, new Transform(position));

		var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-drone"]);
		spriteRenderer.ZIndex = 2;

		_registry.AddComponent<SpriteRenderer>(space_drone, spriteRenderer);
		Collider a = new Collider {
			X = position.X,
			Y = position.Y,
			Width = spriteRenderer.Sprite.Width,
			Height = spriteRenderer.Sprite.Height,
		};
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

		var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bg"], fitScreen: true);
		spriteRenderer.ZIndex = 1;

		_registry.AddComponent<Transform>(background, new Transform(position));
		_registry.AddComponent<SpriteRenderer>(background, spriteRenderer);
		_registry.AddComponent<Script>(background, new ParallaxScrolling());

		_registry.GetComponent<Script>(background).Awake();

		return background;
	}

	public static ushort CreateSpaceBullet(Vector2 position, Vector2 direction)
	{
		ushort bullet = _registry.CreateEntity();

		var spriteRenderer = new SpriteRenderer(CoreGame.Textures["space-bullet1"], fitScreen: false);
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

		_registry.GetComponent<Script>(bullet).Awake();
		return bullet;
	}
}

public class SpaceDroneController : Script {
	// __Fields__

	private RigidBody _rb;
	private Transform _tr;
	private SpriteRenderer _sp;

	private float _directionX;
	private float _directionY;
	private float _speed = 35f;
	private bool _canShoot;
	private float _fireRate = 0.4f;
	private float _shootTimer;

	// __Methods__

	public override void Awake()
	{
		_rb = GetComponent<RigidBody>();
		_tr = GetComponent<Transform>();
		_sp = GetComponent<SpriteRenderer>();

      PhysicsSystem.CollidingObjects += DestroyEntities;
	}

	public override void Update(float deltaTime)
	{

		_shootTimer += deltaTime;
		_canShoot = IsAbleToShoot();
		_directionX = Input.RawHorizontal;
		_directionY = Input.RawVertical;

		Vector2 move_direction = new Vector2(_directionX, _directionY);
		move_direction = move_direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(move_direction);
		_rb.Velocty = move_direction * _speed * deltaTime;

		if (Input.MouseHeld(MouseButton.LeftButton) && _canShoot)
		{
			ShootBullet();
			_canShoot = false;
		}
	}

   public void DestroyEntities(ushort a, ushort b)
	{
      if (a == 0 || b == 0) return;

      CoreGame.Registry.DestroyEntity(a);
      CoreGame.Registry.DestroyEntity(b);
	}

	private void ShootBullet()
	{
		Vector2 bullet_origin = _tr.Position + new Vector2(_sp.Sprite.Width / 2, 0f) - new Vector2(8f, 0f);
		Vector2 bullet_dir = Vector2.Normalize(Input.MousePosition - bullet_origin);
		EntityCreator.CreateSpaceBullet(bullet_origin, bullet_dir);
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

	public override void Awake()
	{
		_imageOne = GetComponent<SpriteRenderer>();
		_transform = GetComponent<Transform>();
	}

	public override void Update(float deltaTime)
	{
		_transform.Position -= new Vector2(0.2f, 0f);
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
	private float _speed = 20;
	private bool _alive = true;

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
      if (c.Entity != 0)
      {
         CoreGame.Registry.DestroyEntity(c.Entity);
         CoreGame.Registry.DestroyEntity(Entity);
      }
   }
}