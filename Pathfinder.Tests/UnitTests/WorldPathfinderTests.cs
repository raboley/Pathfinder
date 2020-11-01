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
            var traveler = new Traveler {CurrentZoneName = start};

            var got = WorldPathfinder.FindWorldPathToZone(world, start, end);

            var want = new List<Zone>
            {
                world.GetZoneByName("A"),
                world.GetZoneByName("C"),
                world.GetZoneByName("D"),
            };

            Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i].Name, got[i].Name);
            }
        }
    }
}