using Pathfinder.WorldMap;

namespace Pathfinder.PrintConsole
{
    public class PrintKnown : BasePrinter
    {
        public string UnknownNode { get; } = "  ?  " + NodeSeparator;

        public override string PrintNode(GridNode gridNode)
        {
            if (gridNode.Unknown)
                return UnknownNode;

            return base.PrintNode(gridNode);
        }
    }
}