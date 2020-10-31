using System.Numerics;
using Pathfinder.Map.WorldMap;
using Pathfinder.Pathing;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PathfinderTests
    {
        [Fact]
        public void TestFindPath()
        {
            /*
             * Visualization of the MapGrid.
             * p = path
             * --------------------------
             * |    |    |    | end|    |
             * --------------------------
             * |    |    |  p |    |    |
             * --------------------------
             * |    |    |0,0p|    |    |
             * --------------------------
             * |    |  p |    |    |    |
             * --------------------------
             * |strt|    |    |    |    |
             * --------------------------
             */

            // arrange
            var pathfinding = SetupForPathfinding();

            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(1, 0, 2);
            Vector3[] want =
            {
                startPos,
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                endPos
            };

            // act 
            var got = pathfinding.FindWaypoints(startPos, endPos);

            // assert
            Assert.Equal(want, got);
        }

        [Fact]
        public void TestFindPathAvoidsObstacles()
        {
            /*
             * Visualization of the MapGrid.
             * p = path
             * x = obstacle
-------------------------------
|     |     |     |     |     |
-------------------------------
|  e  |  w  |     |     |     |
-------------------------------
|  x  |  x  |  x  |  w  |     |
-------------------------------
|     |  w  |  w  |     |     |
-------------------------------
|  s  |     |     |     |     |
-------------------------------
             */

            var pathfinding = SetupForPathfinding();

            pathfinding.ZoneMap.AddUnWalkableNode(Vector3.Zero);
            pathfinding.ZoneMap.AddUnWalkableNode(new Vector3(-1, 0, 0));
            pathfinding.ZoneMap.AddUnWalkableNode(new Vector3(-2, 0, 0));
            pathfinding.ZoneMap.AddUnWalkableNode(new Vector3(-3, 0, 0));
            pathfinding.ZoneMap.AddUnWalkableNode(new Vector3(-4, 0, 0));


            string example = pathfinding.ZoneMap.Print();

            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(-2, 0, 1);
            Vector3[] want =
            {
                startPos,
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 1),
                endPos
            };

            // act 
            var got = pathfinding.FindWaypoints(startPos, endPos);

            // assert
            Assert.Equal(want, got);
        }

        private static Pathfinding SetupForPathfinding()
        {
            var pathfinding = new Pathfinding();
            var grid = ZoneMap.NewGridFromVector2(new Vector2(5, 5));
            pathfinding.ZoneMap = grid;
            return pathfinding;
        }
    }
}