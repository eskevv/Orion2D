using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace Orion2D;

public class Explosions : Script {

   private SpriteRenderer _sp;

   private Random _r = new Random();

   private float _lifeTime;
   private float _maxLifeTime;

   private static SoundEffect _hitFx = CoreGame.Sounds["explosion-hit"];

   public override void Awake()
   {
      _sp = GetComponent<SpriteRenderer>();
   }

   public override void Start()
   {
      _maxLifeTime = 1.4f;
      _hitFx.Play();
   }

   public override void Update(float deltaTime)
   {
      _lifeTime += deltaTime;
      float relAge = _lifeTime / _maxLifeTime;

      _sp.Scale = new Vector2((1f - relAge) + 0.1f, (1f - relAge) + 0.1f);

      byte alphaC = (byte)(255 * ((1f - relAge) + 0.1f));
      _sp.Color = new Color(_sp.Color.R, _sp.Color.G, _sp.Color.B, alphaC);

      if (_lifeTime >= _maxLifeTime)
      {
         CoreGame.Registry.DestroyEntity(Entity);
      }
   }
}