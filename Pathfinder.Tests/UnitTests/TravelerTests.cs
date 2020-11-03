using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class TravelerTests
    {
        [Fact]
        public void NavigatorFindsPathToWaypoint()
        {
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(2, 0, 2);
            Vector3[] want =
            {
                // startPos, -- Don't think I actually want to have the start if I can swing it, so that warping is easier.
                new Vector3(1f, 0f, 1f),
                endPos
            };

            var zone = new Zone("tests");
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
            traveler.CurrentZone = new Zone("test") {Map = grid};
            traveler.Position = Vector3.One;

            traveler.DiscoverAllNodes();


            Assert.Empty(traveler.CurrentZone.Map.UnknownNodes);

            string got = grid.PrintKnown();
            Assert.Equal(want, got);
        }

        [Fact]
        public void AllBorderZonesContainsAllBorderPoints()
        {
            var traveler = new Traveler {CurrentZone = ExampleWorld.ZoneB()};
            var want = new List<Vector3>
            {
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(1, 0, 0)
            };
            var got = traveler.AllBorderZonePoints;

            Assert.Equal(want, got);
        }

        [Fact]
        public void GetZoneBorderFromPointCanGetBorder()
        {
            const string targetZoneName = "B";
            var want = ExampleWorld.ZoneA().Boundaries.Find(b => b.ToZone == targetZoneName);
            var fromPosition = want.FromPosition;
            var targetPosition = want.ToPosition;
            var traveler = new Traveler
            {
                CurrentZone = ExampleWorld.ZoneA(),
                Position = Vector3.Zero,
                World = ExampleWorld.Sample()
            };
            var got = traveler.GetZoneBorderToNameFromPoint(fromPosition);

            Assert.Equal(want, got);
        }

        [Fact]
        public void GoToZoneWillActuallyZoneTheTraveler()
        {
            const string targetZoneName = "B";
            var targetPosition = ExampleWorld.ZoneA().Boundaries.Find(b => b.ToZone == targetZoneName).ToPosition;
            var traveler = new Traveler
            {
                CurrentZone = ExampleWorld.ZoneA(),
                Position = Vector3.Zero,
                World = ExampleWorld.Sample()
            };
            traveler.GoToZone(targetZoneName);


            Assert.Equal(targetZoneName, traveler.CurrentZone.Name);
            Assert.Equal(targetPosition, traveler.Position);
        }

        [Fact]
        public void WalkThroughZonesTravelsThroughZones()
        {
            const string start = "A";
            const string end = "D";

            // setup the world
            var world = ExampleWorld.Sample();
            var traveler = new Traveler(start, world, Vector3.Zero);

            var zones = new List<Zone>
            {
                world.GetZoneByName("A"),
                world.GetZoneByName("C"),
                world.GetZoneByName("D")
            };
            traveler.WalkThroughZones(zones);

            Assert.Equal(end, traveler.CurrentZone.Name);
        }

        [Fact]
        public void FindClosestSignetPersonWhenTwoZonesAway()
        {
            var want = ExampleWorld.BastokSignetPerson();
            var traveler = SetupWorld();

            var got = traveler.SearchForClosestSignetNpc("Bastok");
            Assert.Equal(want, got);
        }

        private static Traveler SetupWorld()
        {
            var world = ExampleWorld.Sample();
            const string start = "A";
            var traveler = new Traveler(start, world, Vector3.Zero);
            return traveler;
        }
    }
}