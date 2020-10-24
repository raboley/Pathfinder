using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public interface INodePrinter
    {
        string PrintNode(Node node);
    }

    public abstract class BasePrinter : INodePrinter
    {
        
        public virtual string PrintNode(Node node)
        {
            if (NotWalkable(node))
                return ObstacleNode;

            return WalkableNode;
        }

        private static bool NotWalkable(Node node)
        {
            return !node.walkable;
        }
        public string WalkableNode { get; } = "     " + NodeSeparator;
        public string ObstacleNode { get; } = "  x  " + NodeSeparator;
        public static string NodeSeparator { get; } = "|";
    }
}