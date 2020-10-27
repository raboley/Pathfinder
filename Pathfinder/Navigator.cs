using System.Numerics;

namespace Pathfinder
{
    public class Navigator
    {
        public Vector3[] WalkToWaypoint(Vector3 waypoint)
        {
            var findWaypoints = Pathfinder.FindWaypoints(Position, waypoint);
            Position = waypoint;
            return findWaypoints;
        }

        public Pathfinding Pathfinder { get; set; }
        public Vector3 Position { get; set; }
    }
}