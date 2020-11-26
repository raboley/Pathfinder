using Pathfinder.Map;

namespace Pathfinder.Pathing
{
    public interface IPathfindingStyle
    {
        bool PathIsFound(Node currentGridNode, Node targetGridNode);
    }
}