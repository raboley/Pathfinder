using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Newtonsoft.Json;
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

        public ConcurrentQueue<Vector3> PointsToExplore { get; set; } = new ConcurrentQueue<Vector3>();

        public Vector3? NextPointToExplore { get; set; } = null;

        public List<Vector3> Explored { get; set; } = new List<Vector3>();

        public List<ZoneBoundary> Boundaries { get; set; } = new List<ZoneBoundary>();

        [JsonIgnore] public ZoneMap Map { get; set; }

        [JsonIgnore] public int GCost { get; set; }

        [JsonIgnore] public int FCost => GCost + HCost;

        [JsonIgnore] public int HCost { get; set; }

        [JsonIgnore] public Zone Parent { get; set; }

        [JsonIgnore] public ObservableCollection<Person> Npcs { get; set; } = new ObservableCollection<Person>();

        [JsonIgnore] public ObservableCollection<IEntity> ThingList { get; set; } = new ObservableCollection<IEntity>();

        [JsonIgnore] public int HeapIndex { get; set; }

        public int CompareTo(Zone other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0) compare = HCost.CompareTo(other.HCost);

            return -compare;
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
    }
}