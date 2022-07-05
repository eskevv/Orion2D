using System.Collections.Generic;
using System.Diagnostics;

namespace Orion2D;
public class EntityManager {

   public const ushort MaxEntities = 5000;

   private Queue<ushort> _availableEntities;
   private BitArray[] _signatures;
   private string[] _tags;

   public ushort EntityCount { get; private set; }

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

      _tags = new string[MaxEntities];
   }

   // __Definitions__

   public void AssignTag(ushort entity, string tag)
   {
      _tags[entity] = tag;
   }

   public string RetrieveTag(ushort entity)
   {
      return _tags[entity];
   }


   public ushort CreateEntity()
   {
      Debug.Assert(EntityCount < MaxEntities, "Too many entities in existence.");
      EntityCount++;
      return _availableEntities.Dequeue();
   }

   public void DestroyEntity(ushort entity)
   {
      Debug.Assert(entity < MaxEntities, "Entity out of range.");
      _signatures[entity].Reset();
      _tags[entity] = "void";
      EntityCount--;
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
}