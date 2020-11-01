using System.Collections.Generic;
using Pathfinder.Map;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldPathfinderTests
    {
        [Fact]
        public void CanPathFromWorldList()
        {
            var start = "A";
            var end = "D";

            var world = World.Sample();
            var traveler = new Traveler();
            traveler.CurrentZoneName = start;

            var worldPathfinder = new WorldPathfinder {World = world};
            worldPathfinder.FindWorldPathToZone(start, end);

            var got = worldPathfinder.ZonesToTravelThrough;

            var want = new List<Zone>
            {
                world.Zones.Find(z => z.Name == "A"),
                world.Zones.Find(z => z.Name == "C"),
                world.Zones.Find(z => z.Name == "D"),
            };

            Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i].Name, got[i].Name);
            }
        }
    }
}