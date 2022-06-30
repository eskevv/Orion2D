using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Orion2D;
public class MovementSystem : ComponentSystem
{
   // __Methods__

   public void Update()
   {
      foreach (var item in Entities)
      {
         var transform = CoreGame.Registry.GetComponent<Transform>(item);
         var rigid_body = CoreGame.Registry.GetComponent<RigidBody>(item);

         transform.Position += rigid_body.Velocty;
      }
   }
}

public class RenderSystem : ComponentSystem
{
   // __Methods__

   public void Render(SpriteBatch spriteBatch)
   {
      var z_list = Entities.ToList();
      z_list.Sort((x, y) =>
      {
         var renderer_one = CoreGame.Registry.GetComponent<SpriteRenderer>(x);
         var renderer_two = CoreGame.Registry.GetComponent<SpriteRenderer>(y);
         return renderer_one.ZIndex.CompareTo(renderer_two.ZIndex);
      });

      spriteBatch.Begin();
      foreach (var item in z_list)
      {
         var renderer = CoreGame.Registry.GetComponent<SpriteRenderer>(item);
         var transform = CoreGame.Registry.GetComponent<Transform>(item);

         Vector2 position = transform.Position;
         Texture2D sprite = renderer.Sprite;
         Color colr = renderer.Color;
         float rota = renderer.Rotation;
         Rectangle? src = renderer.SrcRect;
         Vector2 origin = renderer.Origin;
         Vector2 scale = renderer.Scale;

         if (renderer.Maximized)
         {
            Rectangle dst = new Rectangle(0, 0, CoreGame.ScreenWidth, CoreGame.ScreenHeight);
            spriteBatch.Draw(sprite, dst, src, colr, rota, origin, SpriteEffects.None, 0f);
         }
         else
         {
            spriteBatch.Draw(sprite, transform.Position, src, colr, rota, origin, scale, SpriteEffects.None, 0f);
         }

      }
      spriteBatch.End();
   }
}

public class ScriptSystem : ComponentSystem
{
   // __Methods__

   public void Update()
   {
      IEnumerable<ushort> entities = Entities;
      foreach (var item in entities.Reverse())
      {
         var script = CoreGame.Registry.GetComponent<Script>(item);

         script.Update();
      }
   }
}