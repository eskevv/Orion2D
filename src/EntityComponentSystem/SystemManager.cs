using System.Collections.Generic;
using System.Diagnostics;

namespace Orion2D;
public class SystemManager {
	// __Fields__

	Dictionary<string, BitArray> _signatures;
	Dictionary<string, ComponentSystem> _systems;

	public SystemManager()
	{
		_signatures = new Dictionary<string, BitArray>();
		_systems = new Dictionary<string, ComponentSystem>();
	}

	// __Methods__

	public T RegisterSystem<T>() where T : ComponentSystem, new()
	{
		string type_name = typeof(T).Name;
		Debug.Assert(!_systems.ContainsKey(type_name), "Registering system more than once.");
		System.Console.WriteLine($"Systems[{type_name}] - Registered Successfully.");

		ComponentSystem system = new T();
		_systems[type_name] = system;
		return (T)system;
	}

	public void SetSignature<T>(BitArray signature)
	{
		string type_name = typeof(T).Name;
		Debug.Assert(_systems.ContainsKey(type_name), "System used before registered.");

		_signatures[type_name] = signature;
	}

	public void CleanEntityFromSystems(ushort entity)
	{
		foreach (var item in _systems)
		{
			var system = item.Value;
			system.Entities.Remove(entity);
		}
	}

	public void UpdateEntityReferences(ushort entity, BitArray entitySignature)
	{
		foreach (var item in _systems)
		{
			string type_name = item.Key;
			ComponentSystem system = item.Value;
			BitArray system_signature = _signatures[type_name];

			if (entitySignature.Includes(system_signature))
			{
				if (system.Entities.Add(entity))
				{
					System.Console.WriteLine($"\nEntity Signature: {entitySignature} AND System Signature: {system_signature}");
					System.Console.WriteLine($"Added Entities[{entity}] TO: Systems[{system.GetType().Name}]");
					System.Console.WriteLine($"Systems[{system.GetType().Name}] EntityCount = {system.Entities.Count}");
					system.UpdatedSet = true;
				}
			}
			else if (system.Entities.Remove(entity))
			{
				System.Console.WriteLine($"\nRemoved Entities[{entity}] FROM: Systems[{system.GetType().Name}]");
				System.Console.WriteLine($"Systems[{system.GetType().Name}] EntityCount = {system.Entities.Count}");
			}
		}
	}
}