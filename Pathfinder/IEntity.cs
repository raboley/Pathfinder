using System.Numerics;

namespace Pathfinder
{
    public interface IEntity
    {
        string Name { get; set; }
        Vector3 Position { get; set; }
        string MapName { get; set; }
    }
}