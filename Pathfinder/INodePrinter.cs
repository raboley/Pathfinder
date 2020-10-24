using System.Numerics;

namespace Pathfinder.Pathfinder
{
    public interface INodePrinter
    {
        string PrintNode(Node node, string result);
    }
}