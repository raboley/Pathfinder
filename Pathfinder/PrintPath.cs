using System;
using System.Linq;
using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public class PrintPath : BasePrinter
    {
        public override string PrintNode(GridNode gridNode)
        {
            if (gridNode.WorldPosition == Start)
                return StartNode;

            if (gridNode.WorldPosition == End)
                return EndNode;

            if (isWaypoint(gridNode))
                return WaypointNode;
            
            return base.PrintNode(gridNode);
        }

        private bool isWaypoint(GridNode gridNode)
        {
            if (Path != null)
            {
                if (Path.Length > 0)
                {
                    if (Path.Contains(gridNode.WorldPosition))
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