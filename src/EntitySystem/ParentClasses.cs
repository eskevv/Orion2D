using System.Collections.Generic;

namespace Orion2D;
public class ComponentSystem
{
    public HashSet<ushort> Entities { get; }

    public ComponentSystem()
    {
        Entities = new HashSet<ushort>();
    }
}

public interface IComponentArray
{
    public bool DestroyIndexedData(ushort entity);
}