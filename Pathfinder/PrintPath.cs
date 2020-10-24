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

        public string PrintNode(Node node, string result = "")
        {
            if (node.walkable == false)
                return result + ObstacleNode;

            if (node.worldPosition == Start)
                return result + StartNode;

            if (node.worldPosition == End)
                return result + EndNode;

            if (isWaypoint(node))
                return result + WaypointNode;

            return result + WalkableNode;
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