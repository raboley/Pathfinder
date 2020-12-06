using System.Collections.Generic;
using System.Linq;
using Pathfinder.Map;
using Pathfinder.People;
using Pathfinder.Persistence;

namespace Pathfinder
{
    public class World
    {
        public List<Zone> Zones { get; set; } = new List<Zone>();
        public List<Person> Npcs { get; set; } = new List<Person>();
        public List<Person> Mobs { get; set; } = new List<Person>();
        public FilePersister ZonePersister { get; set; }
        public PeopleOverseer PeopleManager { get; set; }


        public List<Zone> GetNeighbors(Zone zone)
        {
            var neighbors = new List<Zone>();
            var allNeighborZoneNames = new List<string>();
            // Trying to make this more thread safe than it was before. Without totally ripping out the implementation.
            for (int i = 0; i < zone.Boundaries.Count; i++)
            {
                var neighborName = zone.Boundaries[i].ToZone;

                if (!allNeighborZoneNames.Contains(neighborName))
                    allNeighborZoneNames.Add(neighborName); 
            }

            var distinctNeighborZoneNames = new List<string>();
            foreach (var neighborZoneName in allNeighborZoneNames)
            {
                if (distinctNeighborZoneNames.Contains(neighborZoneName))
                    continue;
                
                distinctNeighborZoneNames.Add(neighborZoneName);
            }
            
            // var distinctNeighborZoneNames = allNeighborZoneNames.GroupBy(x => x).Where(g => g.Count() == 1)
            //     .SelectMany(g => g).ToList();


            foreach (string zoneName in distinctNeighborZoneNames)
            {
                neighbors.Add(GetZoneByName(zoneName));
            }

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

        public void LoadAllZonesToWorld(Zone currentZone)
        {
            Zones = ZonePersister.LoadAllOfType<Zone>();
            Zones.RemoveAll(x => x.Name == currentZone.Name);
            Zones.Add(currentZone);
        }

        public List<Person> GetAllNpcs()
        {
            return PeopleManager.GetAllPeople();
        }
    }
}