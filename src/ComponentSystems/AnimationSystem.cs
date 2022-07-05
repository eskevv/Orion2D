namespace Orion2D;
public class AnimationSystem : ComponentSystem {

   public void Update(float deltaTime)
   {
      foreach (var entity in Entities)
      {
         Animator animator = CoreGame.Registry.GetComponent<Animator>(entity);
         SpriteRenderer renderer = CoreGame.Registry.GetComponent<SpriteRenderer>(entity);

         AnimationClip clip = animator.CurrentClip;

         clip.ClipTime += deltaTime;
         if (clip.ClipTime >= clip.FrameSpeed)
         {
            clip.CurrentFrame++;
            clip.CurrentFrame %= clip.MaxFrames;
            clip.ClipTime = 0f;
            renderer.Sprite = clip.Frames[clip.CurrentFrame];
         }
      }
   }
}