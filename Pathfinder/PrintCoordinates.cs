namespace Pathfinder.Pathfinder
{
    public class PrintCoordinates : INodePrinter
    {
        private string _coordSeparator = ",";
        private string _nodeSeparator = "|";

        public string PrintNode(Node node)
        {
            var x = node.worldPosition.X;
            var y = node.worldPosition.Z;
            string xNode = x.ToString().PadLeft(3, ' ');
            string yNode = y.ToString().PadRight(3, ' ');
            
            return xNode + _coordSeparator + yNode + _nodeSeparator;
        }
    }
}