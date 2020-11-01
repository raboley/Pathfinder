using System.Collections.Generic;
using System.Numerics;

namespace Pathfinder.Map
{
    public class Zone : IHeapItem<Zone>
    {
        public string Name { get; set; }

        public Dictionary<string, List<ZoneBoundary>> Boundaries { get; set; } =
            new Dictionary<string, List<ZoneBoundary>>();

        public ZoneMap Map { get; set; }
        public int GCost { get; set; }
        public int FCost => GCost + HCost;
        public int HCost { get; set; }
        public Zone Parent { get; set; }


        public int CompareTo(Zone other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0) compare = HCost.CompareTo(other.HCost);

            return -compare;
        }

        public int HeapIndex { get; set; }

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
                Boundaries = new Dictionary<string, List<ZoneBoundary>>();

            if (Boundaries.ContainsKey(ZonesTo))
            {
                zoneBoundaries = Boundaries[ZonesTo];
                zoneBoundaries.Add(zoneBoundary);
                Boundaries[ZonesTo] = zoneBoundaries;
                return;
            }

            zoneBoundaries = new List<ZoneBoundary> {zoneBoundary};
            Boundaries.Add(ZonesTo, zoneBoundaries);
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
            var zone = new Zone();
            zone.Name = "bastok_mines";
            zone.Boundaries = new Dictionary<string, List<ZoneBoundary>>
            {
                ["bastok_mines"] = new List<ZoneBoundary>
                {
                    new ZoneBoundary
                    {
                        FromZone = "bastok_mines",
                        FromPosition = new Vector3(1, 0, 0),
                        ToZone = "mob_zone",
                        ToPosition = new Vector3(-1, 0, 0)
                    }
                }
            };

            zone.Map = ZoneMap.TinyMap();

            return zone;
        }
    }
}