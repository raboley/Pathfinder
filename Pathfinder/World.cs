using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.Travel;

namespace Pathfinder
{
    public class World
    {
        public List<Zone> Zones { get; set; }

        public Traveler Traveler { get; set; }

        public List<Zone> GetNeighbors(Zone zone)
        {
            List<Zone> neighbors = new List<Zone>();
            foreach (var zoneBoundary in zone.Boundaries)
            {
                neighbors.Add(GetZone(zoneBoundary.ToZone));
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


        private static Zone ZoneA()
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
            return zone;
        }

        private static Zone ZoneB()
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
                }
            };
            return zone;
        }

        private static Zone ZoneC()
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
                }
            };
            return zone;
        }

        private static Zone ZoneD()
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
                }
            };
            return zone;
        }

        private static Zone ZoneE()
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

        public Zone GetZone(string zoneName)
        {
            return Zones.Find(z => z.Name == zoneName);
        }
    }

    public class WorldPathfinder
    {
        public World World { get; set; }
        public List<Zone> ZonesToTravelThrough { get; set; }

        public void FindWorldPathToZone(string end)
        {
            // Setup
            var route = new List<Zone>();
            var failedToFindRoute = true;

            var start = World.Traveler.CurrentZoneName;
            var startZone = World.GetZone(start);

            var endZone = World.GetZone(end);

            var openSet = new Heap<Zone>(World.Zones.Count);
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
                foreach (var neighbour in World.GetNeighbors(currentZone))
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
            if (failedToFindRoute) return;

            var zonesToTravelThrough = RetracePath(startZone, endZone);
            ZonesToTravelThrough = zonesToTravelThrough;
        }

        private List<Zone> RetracePath(Zone startZone, Zone endZone)
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


        private int GetDistance(Zone currentZone, Zone neighbour)
        {
            // don't know how to calc this yet, probably gotta store the cost in the ZoneBoundary once I have calculated
            // Paths between boundaries.
            return 1;
        }
    }
}