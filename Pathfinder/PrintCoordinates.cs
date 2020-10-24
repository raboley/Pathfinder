namespace Pathfinder.Pathfinder
{
    public class PrintCoordinates : BasePrinter
    {
        private string _coordSeparator = ",";

        public override string PrintNode(Node node)
        {
            var x = node.worldPosition.X;
            var y = node.worldPosition.Z;
            string xNode = x.ToString().PadLeft(3, ' ');
            string yNode = y.ToString().PadRight(3, ' ');
            
            return xNode + _coordSeparator + yNode + NodeSeparator;
        }
    }
}