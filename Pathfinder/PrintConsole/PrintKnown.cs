using Pathfinder.WorldMap;

namespace Pathfinder.PrintConsole
{
    public class PrintKnown : BasePrinter
    {
        public string UnknownNode { get; } = "  ?  " + NodeSeparator;

        public override string PrintNode(WorldMapNode worldMapNode)
        {
            if (worldMapNode.Unknown)
                return UnknownNode;

            return base.PrintNode(worldMapNode);
        }
    }
}