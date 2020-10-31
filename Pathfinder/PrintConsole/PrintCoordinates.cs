using Pathfinder.Map.WorldMap;

namespace Pathfinder.PrintConsole
{
    public class PrintCoordinates : BasePrinter
    {
        private const string _coordSeparator = ",";

        public override string PrintNode(Node node)
        {
            float x = node.WorldPosition.X;
            float y = node.WorldPosition.Z;
            string xNode = x.ToString().PadLeft(3, ' ');
            string yNode = y.ToString().PadRight(3, ' ');

            return xNode + _coordSeparator + yNode + NodeSeparator;
        }
    }
}