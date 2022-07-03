using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Orion2D;
public class RenderSystem : ComponentSystem {
	// __Methods__

	public void Render(SpriteBatch spriteBatch)
	{
		RecentUpdates();

		spriteBatch.Begin();
		foreach (var item in Entities)
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

			int sprite_width = sprite.Width * (int)renderer.Scale.X;
			int sprite_height = sprite.Height * (int)renderer.Scale.Y;

			Rectangle dst = new Rectangle((int)position.X, (int)position.Y, sprite_width, sprite_height);

			if (renderer.Maximized)
			{
				dst = new Rectangle((int)position.X, (int)position.Y, CoreGame.ScreenWidth, CoreGame.ScreenHeight);
			}

			spriteBatch.Draw(sprite, dst, src, colr, rota, origin, SpriteEffects.None, 0f);
		}
		spriteBatch.End();
	}

	protected override void PerformSetUpdate()
	{
		var z_list = Entities.ToList();

		z_list.Sort((x, y) => {
			var renderer_one = CoreGame.Registry.GetComponent<SpriteRenderer>(x);
			var renderer_two = CoreGame.Registry.GetComponent<SpriteRenderer>(y);
			return renderer_one.ZIndex.CompareTo(renderer_two.ZIndex);
		});

		Entities = z_list.ToHashSet();
	}
}
