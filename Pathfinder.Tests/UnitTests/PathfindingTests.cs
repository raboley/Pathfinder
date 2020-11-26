using System.Numerics;
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
             * |    |  p |0,0 |    |    |
             * --------------------------
             * |    |  p |    |    |    |
             * --------------------------
             * |strt|    |    |    |    |
             * --------------------------
             */

            // arrange
            var grid = SetupZoneMap.SetupMediumGrid();

            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(1, 0, 2);
            Vector3[] want =
            {
                // startPos,
                new Vector3(-1, 0, -1),
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1),
                endPos
            };

            // act 
            var got = Pathfinding.FindWaypoints(grid, startPos, endPos);

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

            var grid = SetupZoneMap.SetupMediumGrid();

            grid.AddUnWalkableNode(Vector3.Zero);
            grid.AddUnWalkableNode(new Vector3(-1, 0, 0));
            grid.AddUnWalkableNode(new Vector3(-2, 0, 0));
            grid.AddUnWalkableNode(new Vector3(-3, 0, 0));
            grid.AddUnWalkableNode(new Vector3(-4, 0, 0));


            string example = grid.Print();

            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(-2, 0, 1);
            Vector3[] want =
            {
                // startPos,
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(-1, 0, 1),
                endPos
            };

            // act 
            var got = Pathfinding.FindWaypoints(grid, startPos, endPos);

            // assert
            Assert.Equal(want, got);
        }


        [Fact]
        public void FavorsKnownNodesOverUnknownNodes()
        {
            /*
             * Visualization of the MapGrid.
             * p = path
             * x = obstacle
-------------------------------
|     |  e  |     |     |     |
-------------------------------
|  ?  |  ?  |  p  |     |     |
-------------------------------
|  ?  |  ?  |  p  |     |     |
-------------------------------
|  ?  |  p  |     |     |     |
-------------------------------
|  s  |     |     |     |     |
-------------------------------
             */

            var grid = SetupZoneMap.SetupMediumGrid();
            grid.AddKnownNode(new Vector3(0, 0, 0));
            grid.AddKnownNode(new Vector3(0, 0, 1));
            grid.AddKnownNode(new Vector3(0, 0, 2));
            grid.AddKnownNode(new Vector3(0, 0, -1));
            grid.AddKnownNode(new Vector3(0, 0, -2));


            grid.AddKnownNode(new Vector3(-1, 0, -2));
            grid.AddKnownNode(new Vector3(-1, 0, 2));

            // string example = grid.Print();

            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(-1, 0, 2);
            Vector3[] want =
            {
                // startPos,
                new Vector3(-1, 0, -1),
                Vector3.Zero,
                new Vector3(0, 0, 1),
                endPos
            };

            // act 
            var got = Pathfinding.FindWaypoints(grid, startPos, endPos);

            // assert
            Assert.Equal(want, got);
        }
    }
}