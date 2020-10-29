namespace Pathfinder
{
    public interface INodePrinter
    {
        string PrintNode(GridNode gridNode);
    }

    /// <summary>
    ///     Contains the implementation for an INodePrinter that will print if a gridNode is walkable or not.
    /// </summary>
    public abstract class BasePrinter : INodePrinter
    {
        public string WalkableNode { get; } = "     " + NodeSeparator;
        public string ObstacleNode { get; } = "  x  " + NodeSeparator;
        public static string NodeSeparator { get; } = "|";

        public virtual string PrintNode(GridNode gridNode)
        {
            if (NotWalkable(gridNode))
                return ObstacleNode;

            return WalkableNode;
        }

        private static bool NotWalkable(GridNode gridNode)
        {
            return !gridNode.Walkable;
        }
    }
}