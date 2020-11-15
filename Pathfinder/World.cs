using System.Collections.Generic;
using System.Linq;
using Pathfinder.Map;
using Pathfinder.People;

namespace Pathfinder
{
    public class World
    {
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public List<Person> Npcs { get; set; } = new List<Person>();

        public List<Zone> GetNeighbors(Zone zone)
        {
            var neighbors = new List<Zone>();
            var allNeighborZoneNames = zone.Boundaries.Select(b => b.ToZone).ToList();
            var distinctNeighborZoneNames = allNeighborZoneNames.GroupBy(x => x).Where(g => g.Count() == 1)
                .SelectMany(g => g).ToList();


            foreach (string zoneName in distinctNeighborZoneNames) neighbors.Add(GetZoneByName(zoneName));

            return neighbors;
        }


        public static World FinalFantasy()
        {
            var zones = new List<Zone> {Zone.BastokMines()};
            var world = new World {Zones = zones};

            return world;
        }


        public Zone GetZoneByName(string zoneName)
        {
            return Zones.Find(z => z.Name == zoneName);
        }
    }
}