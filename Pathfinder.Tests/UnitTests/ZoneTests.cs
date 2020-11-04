using System.Collections.Generic;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class ZoneTests
    {
        [Fact]
        public void CanCreateAZoneWithConstructor()
        {
            var name = "TestMap";
            var map = SetupZoneMap.SetupSmallGrid();
            var boundaries = new List<ZoneBoundary>();
            var got = new Zone(name)
            {
                Map = map,
                Boundaries = boundaries
            };

            Assert.Equal(name, got.Name);
            Assert.Equal(map, got.Map);
            Assert.Equal(boundaries, got.Boundaries);
        }
    }
}