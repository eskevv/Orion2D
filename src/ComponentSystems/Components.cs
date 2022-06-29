using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Gravity
{
    public Vector2 Force { get; set; }
}

public class Transform
{
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

public class RigidBody
{
    public Vector2 Velocty { get; set; }
    public Vector2 Acceleration { get; set; }

    public RigidBody(Vector2 velocity, Vector2 acceleration)
    {
        Velocty = velocity;
        Acceleration = acceleration;
    }
}

public class SpriteRenderer
{
    public Texture2D Sprite { get; set; }
    public Color Color { get; set; }

    public SpriteRenderer(Texture2D sprite)
    {
        Sprite = sprite;
        Color = Color.White;
    }
}
