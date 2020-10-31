using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Pathfinder.Pathing;
using Pathfinder.Properties;

namespace Pathfinder.Travel
{
    public class Traveler : INotifyPropertyChanged
    {
        public readonly Queue<Vector3> PositionHistory = new Queue<Vector3>();
        private Vector3 _position;
        public Pathfinding Pathfinder { get; set; }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position)) return;
                _position = value;
                PositionHistory.Enqueue(value);
                if (PositionHistory.Count >= 15) PositionHistory.Dequeue();
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

                var position = new Vector3(x, 0, y);
                Position = position;
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
            foreach (var unknownNode in Pathfinder.ZoneMap.UnknownNodes)
            {
                var getToKnowNode = new Vector3(unknownNode.X, unknownNode.Y, unknownNode.Z);
                Pathfinder.ZoneMap.AddKnownNode(getToKnowNode);
                // PathfindAndWalkToFarAwayWorldMapPosition(unknownNode.WorldPosition);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}