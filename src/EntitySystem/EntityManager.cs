using System.Collections.Generic;
using System.Diagnostics;

namespace Orion2D;
public class EntityManager
{
   // __Fields__

   public const ushort MaxEntities = 5000;

   private Queue<ushort> _availableEntities;
   private BitArray[] _signatures;
   private ushort _livingEntityCount;
   private bool[] _awakenedEntities; // script behaviour initialization 

   public EntityManager()
   {
      _availableEntities = new Queue<ushort>();
      for (ushort x = 0; x < MaxEntities; x++)
      {
         _availableEntities.Enqueue(x);
      }

      _signatures = new BitArray[MaxEntities];
      for (ushort x = 0; x < MaxEntities; x++)
      {
         _signatures[x] = new BitArray(ComponentManager.MaxComponents);
      }

      _awakenedEntities = new bool[MaxEntities];
   }

   // __Methods__

   public ushort CreateEntity()
   {
      Debug.Assert(_livingEntityCount < MaxEntities, "Too many entities in existence.");

      _livingEntityCount++;
      return _availableEntities.Dequeue();
   }

   public void DestroyEntity(ushort entity)
   {
      Debug.Assert(entity < MaxEntities, "Entity out of range.");

      _signatures[entity].Reset();

      _livingEntityCount--;
      _availableEntities.Enqueue(entity);
   }

   public void SetSignature(ushort entity, BitArray signature)
   {
      Debug.Assert(entity < MaxEntities, "Entity out of range.");

      _signatures[entity] = signature;
   }

   public BitArray GetSignature(ushort entity)
   {
      Debug.Assert(entity < MaxEntities, "Entity out of range.");

      return _signatures[entity];
   }

   public bool IsAwakened(ushort entity)
   {
      bool awake = _awakenedEntities[entity];
      _awakenedEntities[entity] = true;
      return awake;
   }
}