using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NavigatorTests
    {
        [Fact]
        public void NavigatorFindsPathToWaypoint()
        {
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(2, 0, 2);
            Vector3[] want =
            {
                startPos,
                new Vector3(1f, 0f, 1f),
                endPos
            };

            var zone = new Zone();
            var grid = SetupZoneMap.SetupMediumGrid();
            zone.Map = grid;

            var traveler = new Traveler {Position = startPos, CurrentZone = zone};


            var traveledPath = traveler.PathfindAndWalkToFarAwayWorldMapPosition(endPos);


            Assert.Equal(endPos, traveler.Position);
            Assert.Equal(want.Length, traveledPath.Length);
            for (var i = 0; i < want.Length; i++) Assert.Equal(want[i], traveledPath[i]);
        }

        [Fact(Skip = "Don't need this yet")]
        public void NavigatorDiscoversAllUnknownNodes()
        {
            const string want = @"
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
";
            var grid = SetupZoneMap.SetupSmallGrid();
            var traveler = new Traveler();
            traveler.CurrentZone = new Zone {Map = grid};
            traveler.Position = Vector3.One;

            traveler.DiscoverAllNodes();


            Assert.Empty(traveler.CurrentZone.Map.UnknownNodes);

            string got = grid.PrintKnown();
            Assert.Equal(want, got);
        }

        [Fact]
        public void AllBorderZonesContainsAllBorderPoints()
        {
            var traveler = new Traveler {CurrentZone = World.ZoneB()};
            var want = new List<Vector3>
            {
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(1, 0, 0),
            };
            var got = traveler.AllBorderZonePoints;

            Assert.Equal(want, got);
        }

        [Fact(Skip = "Need to Have Passing border zone tests")]
        public void WalkThroughZonesTravelsThroughZones()
        {
            const string start = "A";
            const string end = "D";

            // setup the world
            var world = World.Sample();
            var traveler = new Traveler(start, world, Vector3.Zero);

            var zones = new List<Zone>
            {
                world.GetZoneByName("A"),
                world.GetZoneByName("B"),
                world.GetZoneByName("C"),
            };
            traveler.WalkThroughZones(zones);

            Assert.Equal(end, traveler.CurrentZone.Name);
        }
    }
}