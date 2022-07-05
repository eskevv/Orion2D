using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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

public class Transform : Component {

   public Vector2 Position { get; set; }

   public Vector2 Scale { get; set; }

   public float Rotation { get; set; }

   public Transform(Vector2 position, float rotation = 0)
   {
      Position = position;
      Scale = Vector2.One;
      Rotation = rotation;
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

public class AnimationClip {

   public Texture2D[] Frames { get; set; }

   public float ClipDuration { get; set; }

   public int MaxFrames { get; set; }

   public float FrameSpeed { get; set; }

   public int CurrentFrame { get; set; }

   public float ClipTime { get; set; }

   public AnimationClip(Texture2D[] frames, float duration)
   {
      ClipDuration = duration;
      Frames = frames;
      MaxFrames = frames.Length;
      FrameSpeed = ClipDuration / frames.Length;
      CurrentFrame = 0;
      ClipTime = 0f;
   }
}

public class Animator : Component {

   private Dictionary<string, AnimationClip> _animations;

   private string _currentAnimationClip;

   public AnimationClip CurrentClip => _animations[_currentAnimationClip];

   public Animator()
   {
      _animations = new Dictionary<string, AnimationClip>();
   }

   // __Definitions__

   public void AddAnimation(AnimationClip animation, string name)
   {
      _animations[name] = animation;

      if (_currentAnimationClip == null)
      {
         _currentAnimationClip = name;
      }
   }

   public void SetAnimation(string name)
   {
      _currentAnimationClip = name;
   }
}