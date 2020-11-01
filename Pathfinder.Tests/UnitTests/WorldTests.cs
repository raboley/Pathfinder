using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
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
            var grid = SetupZoneMap.SetupMediumGrid();
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
            pathfinding.ZoneMap = grid;

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
            var expectedGrid = SetupZoneMap.SetupMediumGrid();
            foreach (var position in path)
            {
                expectedGrid.AddKnownNode(position);
            }

            Assert.Equal(expectedGrid.UnknownNodes.Count, grid.UnknownNodes.Count);
            Assert.Equal(expectedGrid.UnknownNodes, grid.UnknownNodes);
        }

        [Fact]
        public void CanGoToZone()
        {
            // Move to Zone boundary
            var zoneMap = SetupZoneMap.SetupSmallGrid();
            var want = new Vector3(1, 0, 1);
            zoneMap.AddZoneBoundary("B", want);
            var traveler = new Traveler();
            var pathfinder = new Pathfinding();
            pathfinder.ZoneMap = zoneMap;
            traveler.Pathfinder = pathfinder;

            traveler.Position = new Vector3(1, 0, -1);
            traveler.GoToZone("B");
            var got = traveler.Position;


            Assert.Equal(want, got);
        }

        [Fact]
        public void WorldCanInit()
        {
            var zones = new List<Zone> {Zone.BastokMines()};
            var world = new World {Zones = zones};

            Assert.Equal(zones, world.Zones);
        }

        // [Fact]
        // public void ()
        // {
        //     
        //     Assert.Equal(want, got);
        // }

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