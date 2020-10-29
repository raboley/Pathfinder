using System.Numerics;
using Pathfinder.Pathing;

namespace Pathfinder.Travel
{
    public class Traveler
    {
        public Pathfinding Pathfinder { get; set; }
        public Vector3 Position { get; set; }

        public Vector3[] PathfindAndWalkToFarAwayWorldMapPosition(Vector3 waypoint)
        {
            var path = Pathfinder.FindWaypoints(Position, waypoint);
            foreach (var point in path) WalkToPosition(point);

            Position = waypoint;
            return path;
        }


        private void WalkToPosition(Vector3 targetPosition)
        {
            Pathfinder.Grid.AddKnownNode(targetPosition);
        }


        public void DiscoverAllNodes()
        {
            foreach (var unknownNode in Pathfinder.Grid.UnknownNodes)
            {
                var getToKnowNode = new Vector3(unknownNode.X, unknownNode.Y, unknownNode.Z);
                Pathfinder.Grid.AddKnownNode(getToKnowNode);
                // PathfindAndWalkToFarAwayWorldMapPosition(unknownNode.WorldPosition);
            }
        }
    }
}