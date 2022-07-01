using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion2D;
public abstract class Component {
	public ushort Entity { get; set; }
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

	public RigidBody(Vector2 velocity, Vector2 acceleration)
	{
		Velocty = velocity;
		Acceleration = acceleration;
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

	public SpriteRenderer(Texture2D sprite, bool fitScreen = false)
	{
		Sprite = sprite;
		Color = Color.White;
		Origin = Vector2.Zero;
		Scale = Vector2.One;
		Rotation = 0f;
		Maximized = fitScreen;
		SrcRect = null;
		ZIndex = 0;
	}
}

public abstract class Script : Component {
	public virtual void Awake() { }

	public virtual void Update(float deltaTime) { }

	protected T GetComponent<T>()
	{
		return CoreGame.Registry.GetComponent<T>(Entity);
	}
}
