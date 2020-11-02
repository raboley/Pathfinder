using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.People;

namespace Pathfinder
{
    public class ExampleWorld
    {
        public static World Sample()
        {
            var zones = new List<Zone>();
            var a = ZoneA();
            zones.Add(a);

            var b = ZoneB();
            zones.Add(b);

            var c = ZoneC();
            zones.Add(c);

            var d = ZoneD();
            zones.Add(d);

            var e = ZoneE();
            zones.Add(e);

            var bastokSignetNpc = BastokSignetPerson();

            var world = new World {Zones = zones};
            world.Npcs = new List<Person>();
            world.Npcs.Add(bastokSignetNpc);


            return world;
        }

        public static Zone ZoneA()
        {
            const string want = @"
-------------------
|     |     |     |
-------------------
|     |     |  C  |
-------------------
|     |  B  |     |
-------------------
";

            var zone = new Zone();
            zone.Map = ZoneMap.TinyMap();
            zone.Name = "A";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "A",
                    FromPosition = new Vector3(0, 0, -1),
                    ToZone = "B",
                    ToPosition = new Vector3(0, 0, 1)
                },
                new ZoneBoundary
                {
                    FromZone = "A",
                    FromPosition = new Vector3(1, 0, 0),
                    ToZone = "C",
                    ToPosition = new Vector3(-1, 0, 0)
                }
            };
            return zone;
        }

        public static Zone ZoneB()
        {
            const string want = @"
-------------------
|     |  A  |  C  |
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
";

            var zone = new Zone();
            zone.Map = ZoneMap.TinyMap();
            zone.Name = "B";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "B",
                    FromPosition = new Vector3(0, 0, 1),
                    ToZone = "A",
                    ToPosition = new Vector3(0, 0, -1)
                },
                new ZoneBoundary
                {
                    FromZone = "B",
                    FromPosition = new Vector3(1, 0, 1),
                    ToZone = "C",
                    ToPosition = new Vector3(-1, 0, -1)
                },
                new ZoneBoundary
                {
                    FromZone = "B",
                    FromPosition = new Vector3(1, 0, 0),
                    ToZone = "C",
                    ToPosition = new Vector3(-1, 0, 0)
                }
            };
            return zone;
        }

        public static Zone ZoneC()
        {
            const string want = @"
-------------------
|     |     |     |
-------------------
|  A  |     |  D  |
-------------------
|  B  |     |     |
-------------------
";

            var zone = new Zone();
            zone.Map = ZoneMap.TinyMap();
            zone.Name = "C";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(-1, 0, -1),
                    ToZone = "B",
                    ToPosition = new Vector3(1, 0, 1)
                },

                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(-1, 0, 0),
                    ToZone = "A",
                    ToPosition = new Vector3(1, 0, 0)
                },

                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(1, 0, 0),
                    ToZone = "D",
                    ToPosition = new Vector3(-1, 0, 0)
                }
            };
            return zone;
        }

        public static Zone ZoneD()
        {
            const string want = @"
-------------------
|     |     |     |
-------------------
|  C  | sig |     |
-------------------
|     |     |  E  |
-------------------
";

            var zone = new Zone();
            zone.Map = ZoneMap.TinyMap();
            zone.Name = "D";
            zone.Npcs = new ObservableCollection<Person>
            {
                BastokSignetPerson()
            };
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "D",
                    FromPosition = new Vector3(1, 0, -1),
                    ToZone = "E",
                    ToPosition = new Vector3(-1, 0, 1)
                },

                new ZoneBoundary
                {
                    FromZone = "D",
                    FromPosition = new Vector3(-1, 0, 0),
                    ToZone = "C",
                    ToPosition = new Vector3(1, 0, 0)
                }
            };
            return zone;
        }

        public static Zone ZoneE()
        {
            const string want = @"
-------------------
|  D  |     |     |
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
";

            var zone = new Zone();
            zone.Map = ZoneMap.TinyMap();
            zone.Name = "E";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "E",
                    FromPosition = new Vector3(-1, 0, 1),
                    ToZone = "D",
                    ToPosition = new Vector3(1, 0, -1)
                }
            };

            return zone;
        }

        public static Person BastokSignetPerson()
        {
            var npc = new Person(1, "Someone I.M.", Vector3.Zero);
            npc.MapName = "D";
            return npc;
        }
    }
}