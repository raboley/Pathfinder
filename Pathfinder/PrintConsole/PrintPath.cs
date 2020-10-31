using System.Linq;
using System.Numerics;
using Pathfinder.Map.WorldMap;

namespace Pathfinder.PrintConsole
{
    public class PrintPath : BasePrinter
    {
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public Vector3[] Path { get; set; }

        public string WaypointNode { get; set; } = "  w  " + NodeSeparator;

        public string StartNode { get; set; } = "  s  " + NodeSeparator;

        public string EndNode { get; set; } = "  e  " + NodeSeparator;

        public override string PrintNode(Node node)
        {
            if (node.WorldPosition == Start)
                return StartNode;

            if (node.WorldPosition == End)
                return EndNode;

            if (isWaypoint(node))
                return WaypointNode;

            return base.PrintNode(node);
        }

        private bool isWaypoint(Node node)
        {
            if (Path != null)
                if (Path.Length > 0)
                    if (Path.Contains(node.WorldPosition))
                        return true;

            return false;
        }
    }
}