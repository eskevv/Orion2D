using Microsoft.Xna.Framework;

namespace Orion2D;
public class SpaceDroneController : Script {

   private RigidBody _rb;
   private Transform _tr;
   private SpriteRenderer _sp;

   private float _directionX;
   private float _directionY;
   private float _speed = 20f;
   private bool _canShoot;
   private float _fireRate = 10f;
   private float _shootTimer;

   // __Definitions__

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
      ushort bullet = Factory.CreateSpaceBullet(bullet_origin, bullet_dir);
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