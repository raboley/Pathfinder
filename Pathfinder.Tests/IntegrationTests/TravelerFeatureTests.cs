using System.Numerics;
using Pathfinder.Map;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class TravelerFeatureTests
    {
        [Fact]
        public void GetSignet()
        {
            var player = new Player();
            player.Nation = "Bastok";
            const string start = "A";
            player.Traveler = SetupTraveler(start);

            player.GetSignet();

            Assert.True(player.HasSignet);
        }

        [Fact]
        public void WalkFromWoodsToCity()
        {
            const string start = "A";
            const string end = "D";

            // setup the world
            var traveler = SetupTraveler(start);
            traveler.WalkToZone(end);


            Assert.Equal(end, traveler.CurrentZone.Name);
        }

        [Fact]
        public void TravelerMarksNodesAsUnwalkableWhenStuck()
        {
            const string unused = @"
-------------------
|  e  |     |  ?  |
-------------------
|  x  |  x  |     |
-------------------
|  s  |     |  ?  |
-------------------
";
            const string start = "A";

            // setup the world
            var zoneMap = ZoneMap.TinyMap();
            var traveler = new Traveler();
            traveler.CurrentZone = new Zone("test") {Map = zoneMap};
            var actor = new KnownNodeActor(traveler.CurrentZone.Map);
            var watcher = new Watcher(traveler, actor);
            traveler.Position = new Vector3(-1, 0, -1);


            // Create the map the blind traveler will move against
            var knownGrid = ZoneMap.TinyMap();
            knownGrid.AddUnWalkableNode(Vector3.Zero);
            knownGrid.AddUnWalkableNode(new Vector3(-1, 0, 0));

            traveler.BlindGrid = knownGrid;

            traveler.PathfindAndWalkToFarAwayWorldMapPosition(new Vector3(-1, 0, 1));

            var want = new Vector3(-1, 0, 1);
            var got = traveler.Position;


            Assert.Equal(want, got);
            Assert.Equal(2, traveler.CurrentZone.Map.UnknownNodes.Count);
            // Assert.Equal(traveler.PositionHistory);


            // Assert.Equal(want, got);
        }

        private static Traveler SetupTraveler(string start)
        {
            var world = ExampleWorld.Sample();
            var traveler = new Traveler(start, world, Vector3.Zero);
            return traveler;
        }
    }
}