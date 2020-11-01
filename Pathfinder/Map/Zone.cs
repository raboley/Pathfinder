using System.Collections.Generic;
using System.Numerics;

namespace Pathfinder.Map
{
    public class Zone
    {
        public string Name { get; set; }
        public List<ZoneBoundary> Boundaries { get; set; }
        public ZoneMap Map { get; set; }


        public static Zone BastokMines()
        {
            const string want = @"
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