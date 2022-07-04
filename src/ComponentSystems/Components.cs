using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion2D;
public abstract class Component {
   public ushort Entity { get; set; }
}

public class Collider : Component {
   public float X { get; set; }
   public float Y { get; set; }
   public float Width { get; set; }
   public float Height { get; set; }
}

public class Gravity : Component {
   public Vector2 Force { get; set; }
}

public class Transform : Component {
   public Vector2 Position { get; set; }
   public Vector2 Scale { get; set; }
   public float Rotation { get; set; }

   public Transform(Vector2 position)
   {
      Position = position;
      Scale = Vector2.One;
      Rotation = 0f;
   }
}

public class RigidBody : Component {
   public Vector2 Velocty { get; set; }
   public Vector2 Acceleration { get; set; }

   public RigidBody()
   {
      Velocty = Vector2.Zero;
      Acceleration = Vector2.Zero;
   }
}

public class SpriteRenderer : Component {
   public Texture2D Sprite { get; set; }
   public Color Color { get; set; }
   public Vector2 Origin { get; set; }
   public Vector2 Scale { get; set; }
   public float Rotation { get; set; }
   public bool Maximized { get; set; }
   public Rectangle? SrcRect { get; set; }
   public ushort ZIndex { get; set; }
   public bool Additive { get; set; }

   public SpriteRenderer(Texture2D sprite)
   {
      Sprite = sprite;
      Color = Color.White;
      Origin = Vector2.Zero;
      Scale = Vector2.One;
      Rotation = 0f;
      Maximized = false;
      SrcRect = null;
      ZIndex = 0;
      Additive = false;
   }
}

public abstract class Script : Component {
   public virtual void Awake() { }

   public virtual void Update(float deltaTime) { }

   public virtual void OnCollision(Collider c) { }

   protected T GetComponent<T>()
   {
      return CoreGame.Registry.GetComponent<T>(Entity);
   }
}
