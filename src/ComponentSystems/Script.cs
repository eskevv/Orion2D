using Microsoft.Xna.Framework;

namespace Orion2D;
public abstract class Script : Component {

   private bool _started;

   public bool Started()
   {
      bool output = _started;
      _started = true;
      return output;
   }
   
   public virtual void Start() { }
   
   public virtual void Awake() { }

   public virtual void Update(float deltaTime) { }

   public virtual void OnCollision(Collider c) { }

   protected T GetComponent<T>()
   {
      return CoreGame.Registry.GetComponent<T>(Entity);
   }
}
