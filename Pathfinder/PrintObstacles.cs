namespace Pathfinder.Pathfinder
{
    public class PrintObstacles : INodePrinter
    {
        public string PrintNode(Node node, string result)
        {
            if (node.walkable == false)
            {
                result += "  x  |";
            }
            else
            {
                result += "     |";
            }

            return result;
        }
    }
}