using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Pathfinder.Map;

namespace Pathfinder
{
    public class World
    {
        public List<Zone> Zones { get; set; }

        public List<Zone> GetNeighbors(Zone zone)
        {
            List<Zone> neighbors = new List<Zone>();
            var allNeighborZoneNames = zone.Boundaries.Select(b => b.ToZone).ToList();
            var distinctNeighborZoneNames = allNeighborZoneNames.GroupBy(x => x).Where(g => g.Count() == 1)
                .SelectMany(g => g).ToList();


            foreach (var zoneName in distinctNeighborZoneNames)
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

            var world = new World {Zones = zones};


            return world;
        }


        public static Zone ZoneA()
        {
            var zone = new Zone();
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
            zone.Map = ZoneMap.TinyMap();
            return zone;
        }

        public static Zone ZoneB()
        {
            var zone = new Zone();
            zone.Name = "B";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "B",
                    FromPosition = new Vector3(0, 0, 1),
                    ToZone = "A",
                    ToPosition = new Vector3(0, 0, -1),
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
                },
            };
            zone.Map = ZoneMap.TinyMap();
            return zone;
        }

        public static Zone ZoneC()
        {
            var zone = new Zone();
            zone.Name = "C";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(-1, 0, -1),
                    ToZone = "B",
                    ToPosition = new Vector3(1, 0, 1),
                },

                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(-1, 0, 0),
                    ToZone = "A",
                    ToPosition = new Vector3(1, 0, 0),
                },

                new ZoneBoundary
                {
                    FromZone = "C",
                    FromPosition = new Vector3(1, 0, 0),
                    ToZone = "D",
                    ToPosition = new Vector3(-1, 0, 0)
                },
            };
            return zone;
        }

        public static Zone ZoneD()
        {
            var zone = new Zone();
            zone.Name = "D";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "D",
                    FromPosition = new Vector3(1, 0, -1),
                    ToZone = "E",
                    ToPosition = new Vector3(-1, 0, 1),
                },

                new ZoneBoundary
                {
                    ToZone = "C",
                    ToPosition = new Vector3(1, 0, 0),
                    FromZone = "D",
                    FromPosition = new Vector3(-1, 0, 0)
                },
            };
            return zone;
        }

        public static Zone ZoneE()
        {
            var zone = new Zone();
            zone.Name = "E";
            zone.Boundaries = new List<ZoneBoundary>
            {
                new ZoneBoundary
                {
                    FromZone = "E",
                    FromPosition = new Vector3(-1, 0, 1),
                    ToZone = "D",
                    ToPosition = new Vector3(1, 0, -1),
                },
            };
            return zone;
        }

        public Zone GetZoneByName(string zoneName)
        {
            return Zones.Find(z => z.Name == zoneName);
        }
    }

    public static class WorldPathfinder
    {
        public static List<Zone> FindWorldPathToZone(World world, string start, string end)
        {
            // Setup
            var route = new List<Zone>();
            var failedToFindRoute = true;

            var startZone = world.GetZoneByName(start);
            var endZone = world.GetZoneByName(end);

            var openSet = new Heap<Zone>(world.Zones.Count);
            var closedSet = new HashSet<Zone>();

            openSet.Add(startZone);

            // Start pathfinding
            while (openSet.Count > 0)
            {
                // Pop First in Open set, and then add to closed set
                var currentZone = openSet.RemoveFirst();
                closedSet.Add(currentZone);

                // Are we at the end?
                if (currentZone == endZone)
                {
                    failedToFindRoute = false;
                    break;
                }

                // Go through all neighbors of the current zone
                foreach (var neighbour in world.GetNeighbors(currentZone))
                {
                    // if this neighbor is in the closed set, continue
                    if (closedSet.Contains(neighbour)) continue;


                    int newMovementCostToNeighbor = currentZone.GCost + GetDistance(currentZone, neighbour);

                    // if new path to neighbor is shorter OR neighbor is not in openset.
                    if (newMovementCostToNeighbor < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        neighbour.GCost = newMovementCostToNeighbor;
                        neighbour.HCost = GetDistance(neighbour, endZone);
                        neighbour.Parent = currentZone;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }

            // Pathing is done now!
            if (failedToFindRoute) return null;

            var zonesToTravelThrough = RetracePath(startZone, endZone);
            return zonesToTravelThrough;
        }

        private static List<Zone> RetracePath(Zone startZone, Zone endZone)
        {
            var path = new List<Zone>();
            var currentZone = endZone;

            while (currentZone != startZone)
            {
                path.Add(currentZone);
                currentZone = currentZone.Parent;
            }

            // Add the start zone since we reached it
            path.Add(startZone);

            // reverse it since we started at end and went to beginning
            path.Reverse();
            return path;
        }


        private static int GetDistance(Zone currentZone, Zone neighbour)
        {
            // don't know how to calc this yet, probably gotta store the cost in the ZoneBoundary once I have calculated
            // Paths between boundaries.
            return 1;
        }
    }
}