using System.Collections.Generic;
using System.Linq;

namespace Orion2D;
public class ScriptSystem : ComponentSystem {
   // __Methods__

   public void Update(float deltaTime)
   {
      RecentUpdates();

      IEnumerable<ushort> entities = Entities;
      foreach (var item in entities.Reverse())
      {
         var script = CoreGame.Registry.GetComponent<Script>(item);

         script.Update(deltaTime);
      }
   }
}