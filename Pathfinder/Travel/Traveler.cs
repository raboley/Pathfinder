using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Pathfinder.Map;
using Pathfinder.Pathing;
using Pathfinder.People;
using Pathfinder.Properties;

namespace Pathfinder.Travel
{
    public class Traveler : INotifyPropertyChanged
    {
        public readonly Queue<Vector3> PositionHistory = new Queue<Vector3>();
        private Vector3 _position;

        public Traveler()
        {
        }

        public Traveler(string currentZoneName, World world, Vector3 position)
        {
            World = world;
            CurrentZone = world.GetZoneByName(currentZoneName);
            Position = position;
        }

        public List<Zone> ZonesToTravelThrough { get; set; } = new List<Zone>();

        public Zone CurrentZone { get; set; }

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

        public World World { get; set; }

        public List<Vector3> AllBorderZonePoints
        {
            get
            {
                if (CurrentZone?.Boundaries == null)
                    return null;

                return CurrentZone.Boundaries.Select(boundary => boundary.FromPosition).ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Vector3[] PathfindAndWalkToFarAwayWorldMapPosition(Vector3 waypoint)
        {
            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Position, waypoint);
            foreach (var point in path) WalkToPosition(point);

            return path;
        }


        public void WalkToPosition(Vector3 targetPosition)
        {
            while (Position != targetPosition)
            {
                int x = GetNewXorY(Position.X, targetPosition.X);
                int y = GetNewXorY(Position.Z, targetPosition.Z);

                var position = new Vector3(x, 0, y);
                Position = position;
            }

            // Teleport to the new zone if position is equal to a border zone position
            if (AllBorderZonePoints.Contains(Position))
            {
                var boundary = GetZoneBorderToNameFromPoint(Position);
                CurrentZone = World.GetZoneByName(boundary.ToZone);
                Position = boundary.ToPosition;
            }
        }

        public ZoneBoundary GetZoneBorderToNameFromPoint(Vector3 position)
        {
            return CurrentZone.Boundaries.Find(b => b.FromPosition == position);
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

        public void GoToZone(string zone)
        {
            // Already in that zone!
            if (CurrentZone.Name == zone)
                return;

            var pos = GetBorderZonePosition(zone);
            if (pos == null)
                throw new KeyNotFoundException("Don't know where zone: " + zone + " is.");
            PathfindAndWalkToFarAwayWorldMapPosition(pos);
        }

        private Vector3 GetBorderZonePosition(string zone)
        {
            var borderZones = CurrentZone.Boundaries.FindAll(b => b.ToZone == zone);
            Debug.Assert(borderZones != null, nameof(borderZones) + " != null");
            return GetClosestZoneBorder(borderZones);
        }

        private Vector3 GetClosestZoneBorder(List<ZoneBoundary> borderZones)
        {
            ZoneBoundary first = null;
            foreach (var zone in borderZones)
            {
                first = zone;
                break;
            }

            Debug.Assert(first != null, nameof(first) + " != null");
            return first.FromPosition;
        }

        public void DiscoverAllNodes()
        {
            foreach (var unknownNode in CurrentZone.Map.UnknownNodes)
            {
                // Not actually working. Need to make it walk instead of just add. 
                // Then also need to add the concept of unreachable nodes, if there are blocks surrounding a border
                // it should mark them as unreachable, but not necessarily blocked. 
                var getToKnowNode = new Vector3(unknownNode.X, unknownNode.Y, unknownNode.Z);
                CurrentZone.Map.AddKnownNode(getToKnowNode);
                // PathfindAndWalkToFarAwayWorldMapPosition(unknownNode.WorldPosition);
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void WalkToZone(string zoneName)
        {
            var zonesToTravelTo = WorldPathfinder.FindWorldPathToZone(World, CurrentZone.Name, zoneName);
            WalkThroughZones(zonesToTravelTo);
        }

        public void WalkThroughZones(List<Zone> zonesToTravelTo)
        {
            foreach (var zone in zonesToTravelTo) GoToZone(zone.Name);
        }

        public Person SearchForClosestSignetNpc(string nation)
        {
            if (nation == "Bastok")
                return World.Npcs.Find(n => n.Name.Contains("I.M."));

            throw new Exception("Can't find an NPC in global NPCs to give Signet to Nation: " + nation);
        }
    }
}