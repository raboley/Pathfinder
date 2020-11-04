using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.People;
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

        [Fact]
        public void AddingPeopleWithSameIdWillNotAddDuplicates()
        {
            var name = "TestMap";
            var map = SetupZoneMap.SetupSmallGrid();
            var boundaries = new List<ZoneBoundary>();
            var got = new Zone(name)
            {
                Map = map,
                Boundaries = boundaries
            };
            got.Npcs = new ObservableCollection<Person>();
            got.AddNpc(new Person(0, "test person", Vector3.One));
            got.AddNpc(new Person(1, "a person", Vector3.One));
            // second shouldn't add even though position changed due to property
            got.AddNpc(new Person(1, "a person", Vector3.Zero));
            got.AddNpc(new Person(1, "I changed name person", Vector3.One));

            Assert.Equal(2, got.Npcs.Count);
        }
    }
}