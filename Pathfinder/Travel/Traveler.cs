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

            return path;
        }


        private void WalkToPosition(Vector3 targetPosition)
        {
            while (Position != targetPosition)
            {
                int x = GetNewXorY(Position.X, targetPosition.X);
                int y = GetNewXorY(Position.Z, targetPosition.Z);

                Position = new Vector3(x, 0, y);
            }
        }

        private int GetNewXorY(float current, float target)
        {
            int currentInt = GridMath.ConvertFromFloatToInt(current);
            int targetInt = GridMath.ConvertFromFloatToInt(target);

            if (currentInt > targetInt)
                return currentInt - 1;

            if (currentInt < targetInt)
                return currentInt + 1;

            return currentInt;
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