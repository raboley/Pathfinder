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
            /*
             * Visualization of the grid.
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
            
            Vector3[] want = new[]
            {
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
            };
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(1, 0, 2);

            // act 
            Vector3[] got = pathfinding.FindWaypoints(startPos, endPos);
            
            // assert
            Assert.Equal(want, got);
        }
        
        [Fact]
        public void TestFindPathAvoidsObstacles()
        {
            /*
             * Visualization of the grid.
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
            
            pathfinding.Grid.AddUnWalkableNode(Vector3.Zero);
            pathfinding.Grid.AddUnWalkableNode(new Vector3(-1, 0, 0));
            pathfinding.Grid.AddUnWalkableNode(new Vector3(-2, 0, 0));
            pathfinding.Grid.AddUnWalkableNode(new Vector3(-3, 0, 0));
            pathfinding.Grid.AddUnWalkableNode(new Vector3(-4, 0, 0));


            var example = pathfinding.Grid.Print();
            
            Vector3[] want = new[]
            {
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 1),
            };
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(-2, 0, 1);

            // act 
            Vector3[] got = pathfinding.FindWaypoints(startPos, endPos);
            
            // assert
            Assert.Equal(want, got);
        }

        private static Pathfinding SetupForPathfinding()
        {
            Pathfinding pathfinding = new Pathfinding();
            var grid = new Grid(new Vector2(5, 5));
            grid.CreateGrid();
            pathfinding.Grid = grid;
            return pathfinding;
        }
    }
}