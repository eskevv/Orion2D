using Microsoft.Xna.Framework;

namespace Orion2D;

public class ParallaxScrolling : Script {

   private SpriteRenderer _imageOne;
   private Transform _transform;
   private float _speed = 4.6f;

   // __Definitions__

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
