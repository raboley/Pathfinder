using System.Numerics;
using Pathfinder.Pathing;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldTests
    {
        [Fact]
        public void MappingThreadOpens()
        {
            const string expectedUnknowns = @"
-------------------------------
|  ?  |  ?  |  ?  |  ?  |     |
-------------------------------
|  ?  |  ?  |  ?  |     |  ?  |
-------------------------------
|  ?  |  ?  |     |  ?  |  ?  |
-------------------------------
|  ?  |     |  ?  |  ?  |  ?  |
-------------------------------
|     |  ?  |  ?  |  ?  |  ?  |
-------------------------------
";
            // start the watcher and map
            var grid = GridSetup.SetupMediumGrid();
            var actor = new KnownNodeActor(grid);

            // Walk a Path
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(2, 0, 2);
            Vector3[] path =
            {
                startPos,
                new Vector3(-1f, 0f, -1f),
                new Vector3(0),
                new Vector3(1f, 0f, 1f),
                endPos
            };

            var pathfinding = new Pathfinding();
            pathfinding.Grid = grid;

            var navigator = new Traveler();
            var watcher = new Watcher(navigator, actor);
            navigator.Position = new Vector3(-2, 0, -2);
            navigator.Pathfinder = pathfinding;

            // Walked path
            navigator.PathfindAndWalkToFarAwayWorldMapPosition(endPos);
            var got = navigator.PositionHistory.ToArray();
            AssertVectorArrayEqual(path, got);

            // Visual Representation
            string actualUnknonws = grid.PrintKnown();
            Assert.Equal(expectedUnknowns, actualUnknonws);

            // Unknown Nodes correct
            var expectedGrid = GridSetup.SetupMediumGrid();
            foreach (var position in path)
            {
                expectedGrid.AddKnownNode(position);
            }

            Assert.Equal(expectedGrid.UnknownNodes.Count, grid.UnknownNodes.Count);
            Assert.Equal(expectedGrid.UnknownNodes, grid.UnknownNodes);
        }

        private static void AssertVectorArrayEqual(Vector3[] path, Vector3[] got)
        {
            Assert.Equal(path.Length, got.Length);
            for (int i = 0; i < path.Length; i++)
            {
                Assert.Equal(path[i], got[i]);
            }
        }
    }
}