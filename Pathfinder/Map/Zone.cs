using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Pathfinder.People;

namespace Pathfinder.Map
{
    public class Zone : IHeapItem<Zone>
    {
        public Zone(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<ZoneBoundary> Boundaries { get; set; } = new List<ZoneBoundary>();

        public ZoneMap Map { get; set; }
        public int GCost { get; set; }
        public int FCost => GCost + HCost;
        public int HCost { get; set; }
        public Zone Parent { get; set; }
        public ObservableCollection<Person> Npcs { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<IEntity> ThingList { get; set; } = new ObservableCollection<IEntity>();

        public int CompareTo(Zone other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0) compare = HCost.CompareTo(other.HCost);

            return -compare;
        }

        public int HeapIndex { get; set; }

        public void AddNpc(Person person)
        {
            person.MapName = Name;
            if (!Npcs.Contains(person))
                Npcs.Add(person);
        }

        private Person GetNpcFromId(int personId)
        {
            return Npcs.First(n => n.Id == personId);
        }

        public void AddInanimateObject(Person entity)
        {
            entity.MapName = Name;
            ThingList.Add(entity);
        }

        public void AddBoundary(string ZonesFrom, Vector3 ZonesFromPoint, string ZonesTo, Vector3 ZonesToPoint)
        {
            List<ZoneBoundary> zoneBoundaries;
            var zoneBoundary = new ZoneBoundary
            {
                FromZone = ZonesFrom,
                FromPosition = ZonesFromPoint,
                ToZone = ZonesTo,
                ToPosition = ZonesToPoint
            };

            if (Boundaries == null)
                Boundaries = new List<ZoneBoundary>();

            Boundaries.Add(zoneBoundary);
        }


        public static Zone BastokMines()
        {
            const string unused = @"
-------------------
|  ?  |  ?  |  ?  |
-------------------
|  ?  |  ?  |  Z  |
-------------------
|  ?  |  ?  |  ?  |
-------------------
";
            var zone = new Zone("tests");
            zone.Name = "bastok_mines";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "bastok_mines",
                    FromPosition = new Vector3(1, 0, 0),
                    ToZone = "mob_zone",
                    ToPosition = new Vector3(-1, 0, 0)
                }
            };

            zone.Map = ZoneMap.TinyMap();

            return zone;
        }

        public void GetNpcListOrCreateNewIfNotExists()
        {
        }
    }
}