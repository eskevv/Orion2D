using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orion2D;
public class MovementSystem : ComponentSystem
{
    // __Methods__

    public void Update()
    {
        foreach (var item in Entities) {
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
        foreach (var item in Entities) {
            var renderer = CoreGame.Registry.GetComponent<SpriteRenderer>(item);
            var transform = CoreGame.Registry.GetComponent<Transform>(item);

            spriteBatch.Draw(renderer.Sprite, transform.Position, renderer.Color);
        }
    }
}