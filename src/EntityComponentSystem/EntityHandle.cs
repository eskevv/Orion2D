namespace Orion2D;
public class EntityHandle {
   
   private ushort _id;
   private EntityRegistry _registry;

   public EntityHandle(ushort id)
   {
      _id = id;
      _registry = CoreGame.Registry;
   }

   // __Definitions__

   public void AddComponent<T>(T component) where T : Component => _registry.AddComponent<T>(_id, component);

   public T GetComponent<T>() => _registry.GetComponent<T>(_id);

   public void AssignTag(string tagName) => _registry.AssignTag(_id, tagName);

}