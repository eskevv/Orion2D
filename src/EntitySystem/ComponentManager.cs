using System.Collections.Generic;
using System.Diagnostics;

namespace Orion2D;
public class ComponentManager
{
   // __Fields__

   public const byte MaxComponents = 10;

   private Dictionary<string, ushort> _componentTypes;
   private Dictionary<string, IComponentArray> _components;
   private ushort _nextComponentType;

   public ComponentManager()
   {
      _componentTypes = new Dictionary<string, ushort>();
      _components = new Dictionary<string, IComponentArray>();
   }

   // __Methods__

   private ComponentArray<T> GetComponentArray<T>()
   {
      string type_name = typeof(T).Name;
      Debug.Assert(_componentTypes.ContainsKey(type_name), $"Component |{type_name}| not registered before use.");

      return (ComponentArray<T>)_components[type_name];
   }

   public void RegisterComponent<T>()
   {
      string type_name = typeof(T).Name;
      Debug.Assert(!_componentTypes.ContainsKey(type_name), $"Registering component type |{type_name}| more than once.");
      System.Console.WriteLine($"Components[{type_name}] - Registered Successfully.");

      _componentTypes[type_name] = _nextComponentType++;
      _components[type_name] = new ComponentArray<T>();
   }

   public ushort GetComponentType<T>()
   {
      string type_name = typeof(T).Name;
      Debug.Assert(_componentTypes.ContainsKey(type_name), $"Component |{type_name}| not registered before use.");

      return _componentTypes[type_name];
   }

   public void AddComponent<T>(ushort entity, T component) where T : Component
   {
      ComponentArray<T> component_array = GetComponentArray<T>();
      component_array.InsertData(entity, component);
      component.Entity = entity;
   }

   public void RemoveComponent<T>(ushort entity)
   {
      ComponentArray<T> component_array = GetComponentArray<T>();
      component_array.RemoveData(entity);
   }

   public T GetComponent<T>(ushort entity)
   {
      ComponentArray<T> component_array = GetComponentArray<T>();
      return component_array.GetData(entity);

   }

   public void DestroyEntityComponents(ushort entity)
   {
      foreach (var item in _components)
      {
         var component = item.Value;
         bool is_removed = component.DestroyIndexedData(entity);

         if (is_removed)
         {
            Debug.WriteLine($"Successfully destroyed components of entity [{entity}]");
         }
      }
   }
}