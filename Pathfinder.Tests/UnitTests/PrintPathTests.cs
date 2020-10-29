using System.Numerics;
using Pathfinder.PrintConsole;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PrintPathTests
    {
        [Fact]
        public void TestPrintPathPrintsObstacleWhenPresent()
        {
            var node = new WorldMapNode(Vector3.One, false);
            var printer = new PrintPath();
            string want = printer.ObstacleNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }


        [Fact]
        public void TestPrintPathPrintsWalkable()
        {
            var node = new WorldMapNode(Vector3.One);
            var printer = new PrintPath();
            string want = printer.WalkableNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsStart()
        {
            var pathStart = Vector3.One;
            var node = new WorldMapNode(pathStart);
            var printer = new PrintPath {Start = pathStart};
            string want = printer.StartNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsEnd()
        {
            var pathEnd = Vector3.One;
            var node = new WorldMapNode(pathEnd);
            var printer = new PrintPath {End = pathEnd};
            string want = printer.EndNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }

        [Fact]
        public void TestPrintPathPrintsWaypoint()
        {
            var pathWaypoint = Vector3.One;
            var node = new WorldMapNode(pathWaypoint);
            var printer = new PrintPath {Path = new[] {pathWaypoint}};
            string want = printer.WaypointNode;

            string got = printer.PrintNode(node);

            Assert.Equal(want, got);
        }
    }
}