namespace Pathfinder
{
    public class PrintKnown : BasePrinter
    {
        public override string PrintNode(GridNode gridNode)
        {
            if (gridNode.Unknown)
                return UnknownNode;

            return base.PrintNode(gridNode);
        }

        public string UnknownNode { get; } = "  ?  " + NodeSeparator;
    }
}