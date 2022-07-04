using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Orion2D;
public class RenderSystem : ComponentSystem {

   enum BlendMode { Alpha, Additive };

   private BlendMode _currentBlend = BlendMode.Alpha;

   // __Definitions__

   public void Render(SpriteBatch spriteBatch)
   {
      RecentUpdates();

      spriteBatch.Begin();

      DrawTextures(spriteBatch);

      spriteBatch.End();
   }

   private void DrawTextures(SpriteBatch spriteBatch)
   {
      foreach (var item in Entities)
      {
         var renderer = CoreGame.Registry.GetComponent<SpriteRenderer>(item);
         var transform = CoreGame.Registry.GetComponent<Transform>(item);

         RedirectBatchMethod(spriteBatch, renderer);

         Vector2 position = transform.Position;
         Texture2D sprite = renderer.Sprite;

         int sprite_width = (int)(sprite.Width * renderer.Scale.X);
         int sprite_height = (int)(sprite.Height * renderer.Scale.Y);

         Rectangle dst = new Rectangle((int)position.X, (int)position.Y, sprite_width, sprite_height);
         dst = renderer.Maximized ? new Rectangle((int)position.X, (int)position.Y, CoreGame.ScreenWidth, CoreGame.ScreenHeight) : dst;

         spriteBatch.Draw(sprite, dst, renderer.SrcRect, renderer.Color, renderer.Rotation, renderer.Origin, SpriteEffects.None, 0f);
      }
   }

   protected override void PerformSetUpdate()
   {
      var z_list = Entities.ToList();

      z_list.Sort((x, y) => {
         var renderer_one = CoreGame.Registry.GetComponent<SpriteRenderer>(x);
         var renderer_two = CoreGame.Registry.GetComponent<SpriteRenderer>(y);
         int baseComparison = renderer_one.ZIndex.CompareTo(renderer_two.ZIndex);
         return baseComparison == 0 ? x.CompareTo(y) : baseComparison;
      });

      Entities = z_list.ToHashSet();
   }

   private void RedirectBatchMethod(SpriteBatch spriteBatch, SpriteRenderer renderer)
   {
      if (renderer.Additive && _currentBlend == BlendMode.Alpha)
      {
         spriteBatch.End();
         _currentBlend = BlendMode.Additive;
         spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
      }
      else if (!renderer.Additive && _currentBlend == BlendMode.Additive)
      {
         spriteBatch.End();
         _currentBlend = BlendMode.Alpha;
         spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      }
   }
}
