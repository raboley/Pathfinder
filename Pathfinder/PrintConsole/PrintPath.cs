using System.Linq;
using System.Numerics;
using Pathfinder.WorldMap;

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

        public override string PrintNode(WorldMapNode worldMapNode)
        {
            if (worldMapNode.WorldPosition == Start)
                return StartNode;

            if (worldMapNode.WorldPosition == End)
                return EndNode;

            if (isWaypoint(worldMapNode))
                return WaypointNode;

            return base.PrintNode(worldMapNode);
        }

        private bool isWaypoint(WorldMapNode worldMapNode)
        {
            if (Path != null)
                if (Path.Length > 0)
                    if (Path.Contains(worldMapNode.WorldPosition))
                        return true;

            return false;
        }
    }
}