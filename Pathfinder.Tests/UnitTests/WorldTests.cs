using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
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
            var zone = new Zone();
            var grid = SetupZoneMap.SetupMediumGrid();
            zone.Map = grid;
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


            var traveler = new Traveler();
            // Watcher must be watching traveler before it gets it's current position otherwise it won't record the starting
            // position
            var watcher = new Watcher(traveler, actor);
            traveler.CurrentZone = zone;
            traveler.Position = new Vector3(-2, 0, -2);

            // Walked path
            traveler.PathfindAndWalkToFarAwayWorldMapPosition(endPos);
            var got = traveler.PositionHistory.ToArray();
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
            var zone = new Zone();
            zone.Map = SetupZoneMap.SetupSmallGrid();
            zone.Name = "B";

            var want = new Vector3(1, 0, 1);
            var targetZone = "C";
            zone.AddBoundary(zone.Name, want, targetZone, new Vector3(-1, 0, -1));

            var traveler = new Traveler();
            traveler.CurrentZone = zone;

            traveler.Position = new Vector3(1, 0, -1);

            traveler.GoToZone(targetZone);
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