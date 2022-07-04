using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Orion2D;
public class ComponentArray<T> : IComponentArray {
	// __Fields__

	private T[] _componentArray;
	private ushort _size;
	private Dictionary<ushort, ushort> _dataIndexes; // given an entity key
	private Dictionary<ushort, ushort> _entityIndexes; // given a data key

	public ComponentArray()
	{
		_componentArray = new T[EntityManager.MaxEntities];
		_dataIndexes = new Dictionary<ushort, ushort>();
		_entityIndexes = new Dictionary<ushort, ushort>();
	}

	// __Methods__

	public bool DestroyIndexedData(ushort entity)
	{
		if (!_dataIndexes.ContainsKey(entity)) return false;
		RemoveData(entity);
		return true;
	}

	public void InsertData(ushort entity, T component)
	{
		Debug.Assert(!_dataIndexes.ContainsKey(entity), "Component added to same entity more than once.");

		_dataIndexes[entity] = _size;
		_entityIndexes[_size] = entity;
		_componentArray[_size] = component;
		_size++;
	}

	public void RemoveData(ushort entity)
	{
		Debug.Assert(_dataIndexes.ContainsKey(entity), "Removing non-existent component.");

		ushort last_index = (ushort)(_size - 1);
		ushort last_entity = _entityIndexes[last_index];
		ushort removed_data_index = _dataIndexes[entity];

		_componentArray[removed_data_index] = _componentArray[last_index];
		_dataIndexes[last_entity] = removed_data_index;
		_entityIndexes[removed_data_index] = last_entity;

		_dataIndexes.Remove(entity);
		_entityIndexes.Remove(last_index);
		_size--;
	}

	public T GetData(ushort entity)
	{
		Debug.Assert(_dataIndexes.ContainsKey(entity), "Retrieving non-existent component.");
		ushort data_index = _dataIndexes[entity];
		return _componentArray[data_index];
	}

	public bool HasData(ushort entity)
	{
		return _dataIndexes.ContainsKey(entity);
	}

	public ushort GetEntity(T component) // likely unused / obsolete utility
	{
		ushort entity_index = (ushort)_componentArray.ToList().IndexOf(component);
		Debug.Assert(_dataIndexes.ContainsKey(entity_index), "Component does not exist.");
		return _entityIndexes[entity_index];
	}
}