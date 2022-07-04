using System.Collections.Generic;

namespace Orion2D;
public class ComponentSystem {
   // __Fields__

   public HashSet<ushort> Entities { get; set; }
   public bool IsSetUpdated { get; set; }

   public ComponentSystem()
   {
      Entities = new HashSet<ushort>();
   }

   // __Methods__

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
