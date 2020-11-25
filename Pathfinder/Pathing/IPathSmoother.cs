using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;

namespace Pathfinder.Pathing
{
    public interface IPathSmoother
    {
        Vector3[] PathToArray(Vector3 start, Vector3 end, List<Node> path);
    }
}