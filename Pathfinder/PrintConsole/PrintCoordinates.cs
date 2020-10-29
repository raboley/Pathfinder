using Pathfinder.WorldMap;

namespace Pathfinder.PrintConsole
{
    public class PrintCoordinates : BasePrinter
    {
        private const string _coordSeparator = ",";

        public override string PrintNode(GridNode gridNode)
        {
            float x = gridNode.WorldPosition.X;
            float y = gridNode.WorldPosition.Z;
            string xNode = x.ToString().PadLeft(3, ' ');
            string yNode = y.ToString().PadRight(3, ' ');

            return xNode + _coordSeparator + yNode + NodeSeparator;
        }
    }
}