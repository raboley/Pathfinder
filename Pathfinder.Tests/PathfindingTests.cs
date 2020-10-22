using System.IO;
using System.Numerics;
using Pathfinder.Pathfinder;
using Xunit;

namespace Pathfinder.Tests.Pathfinder
{
    public class PathfinderTests
    {
        [Fact]
        public void TestFindPath()
        {
            // setup
            Pathfinding pathfinding = new Pathfinding();
            pathfinding.Grid = new Grid(Vector2.One);
            
        }
    }
}