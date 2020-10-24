using System.Numerics;
using Pathfinder.Pathfinder;
using Xunit;

namespace Pathfinder.Tests.Pathfinder
{
    public class PrintPathTests
    {

        [Fact]
        public void TestPrintPathPrintsObstacleWhenPresent()
        {
            var node = new Node(Vector3.One, false); 
            var printer = new PrintPath();
            var want = printer.ObstacleNode;

            var got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
        
        
        [Fact]
        public void TestPrintPathPrintsWalkable()
        {
            var node = new Node(Vector3.One, true); 
            var printer = new PrintPath();
            var want = printer.WalkableNode;

            var got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
        
        [Fact]
        public void TestPrintPathPrintsStart()
        {
            var pathStart = Vector3.One;
            var node = new Node(pathStart, true);
            var printer = new PrintPath {Start = pathStart};
            var want = printer.StartNode;

            var got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
        
        [Fact]
        public void TestPrintPathPrintsEnd()
        {
            var pathEnd = Vector3.One;
            var node = new Node(pathEnd, true);
            var printer = new PrintPath {End = pathEnd};
            var want = printer.EndNode;

            var got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
        
        [Fact]
        public void TestPrintPathPrintsWaypoint()
        {
            var pathWaypoint = Vector3.One;
            var node = new Node(pathWaypoint, true);
            var printer = new PrintPath {Path = new[] {pathWaypoint}};
            var want = printer.WaypointNode;

            var got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
    }
}