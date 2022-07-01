using System.Collections.Generic;

namespace Orion2D;
public class ComponentSystem {
   // __Fields__

	public HashSet<ushort> Entities { get; set; }
	public bool UpdatedSet { get; set; }

	public ComponentSystem()
	{
		Entities = new HashSet<ushort>();
	}

   // __Methods__

	protected void RecentUpdates()
	{
		if (UpdatedSet)
		{
			PerformSetUpdate();
			UpdatedSet = false;
		}
	}

	protected virtual void PerformSetUpdate() { }
}
