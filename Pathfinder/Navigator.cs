using System.Numerics;

namespace Pathfinder
{
    public class Navigator
    {
        public Vector3[] WalkToWaypoint(Vector3 waypoint)
        {
            var path = Pathfinder.FindWaypoints(Position, waypoint);
            foreach (var point in path)
            {
                Pathfinder.Grid.AddKnownNode(point);
            }

            Position = waypoint;
            return path;
        }

        public void DiscoverAllNodes()
        {
            foreach (var unknownNode in Pathfinder.Grid.UnknownNodes)
            {
                var getToKnowNode = new Vector3(unknownNode.X, unknownNode.Y, unknownNode.Z);
                Pathfinder.Grid.AddKnownNode(getToKnowNode);
                // WalkToWaypoint(unknownNode.WorldPosition);
            }
        }

        public Pathfinding Pathfinder { get; set; }
        public Vector3 Position { get; set; }
    }
}