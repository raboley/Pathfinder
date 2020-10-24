using System;
using System.Linq;
using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public class PrintPath : INodePrinter
    {
        public static PrintPath CreateInstance()
        {
            var printer = new PrintPath
            {
                ObstacleNode = "  x  |",
                WalkableNode = "     |",
                EndNode = "  e  |",
                StartNode = "  s  |",
                WaypointNode = "  w  |"
            };

            return printer;
        }

        public string PrintNode(Node node)
        {
            if (node.walkable == false)
                return ObstacleNode;

            if (node.worldPosition == Start)
                return StartNode;

            if (node.worldPosition == End)
                return EndNode;

            if (isWaypoint(node))
                return WaypointNode;

            return WalkableNode;
        }

        private bool isWaypoint(Node node)
        {
            if (Path != null)
            {
                if (Path.Length > 0)
                {
                    if (Path.Contains(node.worldPosition))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public Vector3[] Path { get; set; }

        public string WaypointNode { get; set; }

        public string StartNode { get; set; }

        public string EndNode { get; set; }

        public string WalkableNode { get; set; }

        public string ObstacleNode { get; set; }
    }
}