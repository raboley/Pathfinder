using System.Collections.Generic;
using Pathfinder.Map;

namespace Pathfinder
{
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