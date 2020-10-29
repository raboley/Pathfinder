using Pathfinder.WorldMap;

namespace Pathfinder.PrintConsole
{
    public interface INodePrinter
    {
        string PrintNode(WorldMapNode worldMapNode);
    }

    /// <summary>
    ///     Contains the implementation for an INodePrinter that will print if a worldMapNode is walkable or not.
    /// </summary>
    public abstract class BasePrinter : INodePrinter
    {
        public string WalkableNode { get; } = "     " + NodeSeparator;
        public string ObstacleNode { get; } = "  x  " + NodeSeparator;
        public static string NodeSeparator { get; } = "|";

        public virtual string PrintNode(WorldMapNode worldMapNode)
        {
            if (NotWalkable(worldMapNode))
                return ObstacleNode;

            return WalkableNode;
        }

        private static bool NotWalkable(WorldMapNode worldMapNode)
        {
            return !worldMapNode.Walkable;
        }
    }
}