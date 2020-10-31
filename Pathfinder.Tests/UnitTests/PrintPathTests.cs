using System.Numerics;
using Pathfinder.Map.WorldMap;
using Pathfinder.PrintConsole;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PrintPathTests
    {
        [Fact]
        public void TestPrintPathPrintsObstacleWhenPresent()
        {
            var node = new Node(Vector3.One, false);
            var printer = new PrintPath();
            string want = printer.ObstacleNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }


        [Fact]
        public void TestPrintPathPrintsWalkable()
        {
            var node = new Node(Vector3.One);
            var printer = new PrintPath();
            string want = printer.WalkableNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsStart()
        {
            var pathStart = Vector3.One;
            var node = new Node(pathStart);
            var printer = new PrintPath {Start = pathStart};
            string want = printer.StartNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsEnd()
        {
            var pathEnd = Vector3.One;
            var node = new Node(pathEnd);
            var printer = new PrintPath {End = pathEnd};
            string want = printer.EndNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsWaypoint()
        {
            var pathWaypoint = Vector3.One;
            var node = new Node(pathWaypoint);
            var printer = new PrintPath {Path = new[] {pathWaypoint}};
            string want = printer.WaypointNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
    }
}