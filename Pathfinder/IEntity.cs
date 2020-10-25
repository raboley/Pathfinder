using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public interface IEntity
    {
        string Name { get; set; }
        Vector3 Position { get; set; }
        string Zone { get; set; }
    }
}