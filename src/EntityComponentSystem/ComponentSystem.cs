using System.Collections.Generic;

namespace Orion2D;
public class ComponentSystem {

   protected EntityRegistry Coordinator => CoreGame.Registry;

   public HashSet<ushort> Entities { get; set; }

   public bool IsSetUpdated { get; set; }

   public ComponentSystem()
   {
      Entities = new HashSet<ushort>();
   }

   // __Definitions__

   protected void RecentUpdates()
   {
      if (IsSetUpdated)
      {
         PerformSetUpdate();
         IsSetUpdated = false;
      }
   }

   protected virtual void PerformSetUpdate() { }
}
