using Pathfinder.Map;

namespace Pathfinder.PrintConsole
{
    public class PrintKnown : BasePrinter
    {
        public string UnknownNode { get; } = "  ?  " + NodeSeparator;

        public override string PrintNode(Node node)
        {
            if (node.Unknown)
                return UnknownNode;

            return base.PrintNode(node);
        }
    }
}