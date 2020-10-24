namespace Pathfinder.Pathfinder
{
    public class PrintCoordinates : INodePrinter
    {
        public string PrintNode(Node node, string result)
        {
            var x = node.worldPosition.X;
            var y = node.worldPosition.Z;
            result += x.ToString().PadLeft(3, ' ') + "," + y.ToString().PadRight(3, ' ') + "|";
            return result;
        }
    }
}