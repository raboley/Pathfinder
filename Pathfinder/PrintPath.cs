using System;
using System.Linq;
using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public class PrintPath : BasePrinter
    {
        public override string PrintNode(Node node)
        {
            if (node.worldPosition == Start)
                return StartNode;

            if (node.worldPosition == End)
                return EndNode;

            if (isWaypoint(node))
                return WaypointNode;
            
            return base.PrintNode(node);
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

        public string WaypointNode { get; set; } = "  w  " + NodeSeparator;

        public string StartNode { get; set; } = "  s  " + NodeSeparator;

        public string EndNode { get; set; } = "  e  " + NodeSeparator;

    }
}