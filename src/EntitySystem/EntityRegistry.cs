namespace Orion2D;
public class EntityRegistry
{
   // __Fields__

   private ComponentManager _componentManager;
   private SystemManager _systemManager;
   private EntityManager _entityManager;

   public EntityRegistry()
   {
      _componentManager = new ComponentManager();
      _systemManager = new SystemManager();
      _entityManager = new EntityManager();
   }

   // __Methods__

   public void DestroyEntity(ushort entity)
   {
      _entityManager.DestroyEntity(entity);
      _componentManager.DestroyEntityComponents(entity);
      _systemManager.CleanEntityFromSystems(entity);
   }

   public void AddComponent<T>(ushort entity, T component) where T : Component
   {
      _componentManager.AddComponent<T>(entity, component);

      BitArray signature = _entityManager.GetSignature(entity);
      ushort component_type = _componentManager.GetComponentType<T>();
      signature.SetBits(component_type);

      _entityManager.SetSignature(entity, signature);
      _systemManager.UpdateEntityReferences(entity, signature);
   }

   public void RemoveComponent<T>(ushort entity)
   {
      _componentManager.RemoveComponent<T>(entity);

      BitArray signature = _entityManager.GetSignature(entity);
      ushort component_type = _componentManager.GetComponentType<T>();
      signature.ClearBits(component_type);

      _entityManager.SetSignature(entity, signature);
      _systemManager.UpdateEntityReferences(entity, signature);
   }

   public ushort CreateEntity() => _entityManager.CreateEntity();

   public void RegisterComponent<T>() => _componentManager.RegisterComponent<T>();

   public T GetComponent<T>(ushort entity) => _componentManager.GetComponent<T>(entity);

   public ushort GetComponentType<T>() => _componentManager.GetComponentType<T>();

   public T RegisterSystem<T>() where T : ComponentSystem, new() => _systemManager.RegisterSystem<T>();

   public void SetSystemSignature<T>(BitArray signature) => _systemManager.SetSignature<T>(signature);

   public bool IsAwakened(ushort entity) => _entityManager.IsAwakened(entity);
}