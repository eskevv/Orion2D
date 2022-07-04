namespace Orion2D;
public class EntityRegistry {
	// __Fields__

	private ComponentManager _componentManager;
	private SystemManager _systemManager;
	private EntityManager _entityManager;

	private ushort[] _entitiesToKill;
	private ushort _size;

   public int TotalEntities => _entityManager.TotalLiving();

	public EntityRegistry()
	{
		_componentManager = new ComponentManager();
		_systemManager = new SystemManager();
		_entityManager = new EntityManager();

		_entitiesToKill = new ushort[EntityManager.MaxEntities];
	}

	// __Methods__

	public void UpdateRegistry()
	{
		for (ushort x = 0; x < _size; x++)
		{
			ushort entity = _entitiesToKill[x];
			_entityManager.DestroyEntity(entity);
			_componentManager.DestroyEntityComponents(entity);
			_systemManager.CleanEntityFromSystems(entity);
		}

		_size = 0;
	}

	public void DestroyEntity(ushort entity)
	{
      for (int x = 0; x < _size; x++)
      {
         if (_entitiesToKill[x] == entity) return;
      }

		_entitiesToKill[_size++] = entity;
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

	public bool HasComponentType<T>(ushort entity) => _componentManager.HasComponentType<T>(entity);

   public void AssignTag(ushort entity, string tag) => _entityManager.AssignTag(entity, tag);

   public string RetrieveTag(ushort entity) => _entityManager.RetrieveTag(entity);
}