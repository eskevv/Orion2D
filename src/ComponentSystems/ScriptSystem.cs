using System.Collections.Generic;
using System.Linq;

namespace Orion2D;
public class ScriptSystem : ComponentSystem {
   
   // __Definitions__

   public void Update(float deltaTime)
   {
      RecentUpdates();

      IEnumerable<ushort> entities = Entities;
      foreach (var item in entities.Reverse())
      {
         var script = Coordinator.GetComponent<Script>(item);

         if (!script.Started())
         {
            script.Start();
         }

         script.Update(deltaTime);
      }
   }
}