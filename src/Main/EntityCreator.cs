using Microsoft.Xna.Framework;

namespace Orion2D;
public static class EntityCreator
{
    private static EntityRegistry _registry => CoreGame.Registry;

    public static ushort CreateSpaceDrone(Vector2 position)
    {
        ushort _space_drone = _registry.CreateEntity();
        _registry.AddComponent<Transform>(_space_drone, new Transform(position));
        _registry.AddComponent<SpriteRenderer>(_space_drone, new SpriteRenderer(CoreGame._textures_["space-drone"]));
        _registry.AddComponent<RigidBody>(_space_drone, new RigidBody(new Vector2(1.1f, 0.2f), new Vector2(1f, 1f)));

        return _space_drone;
    }
}

public class SpaceDone
{
    ushort entity_id;

    public SpaceDone(Vector2 position)
    {
        entity_id = CoreGame.Registry.CreateEntity();
        CoreGame.Registry.AddComponent<Transform>(entity_id, new Transform(position));
        CoreGame.Registry.AddComponent<SpriteRenderer>(entity_id, new SpriteRenderer(CoreGame._textures_["space-drone"]));
        CoreGame.Registry.AddComponent<RigidBody>(entity_id, new RigidBody(new Vector2(1.1f, 0.2f), new Vector2(1f, 1f)));
    }
}