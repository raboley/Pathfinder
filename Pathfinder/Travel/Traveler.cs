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
        private readonly IWalker _walker;
        public readonly Queue<Vector3> PositionHistory = new Queue<Vector3>();
        public Queue<Vector3> _pathToWalk;
        public ZoneMap BlindGrid;
        public Vector3 goalPosition;

        public Traveler()
        {
        }

        public Traveler(string currentZoneName, World world, IWalker walker)
        {
            _walker = walker;
            _walker.IsStuck += WalkerOnIsStuck;
            _walker.PropertyChanged += WalkerOnPropertyChanged;
            World = world;
            CurrentZone = world.GetZoneByName(currentZoneName);
            Position = walker.CurrentPosition;
            CurrentZone.Map.AddKnownNode(Position);
        }

        public List<Vector3> AllBorderZonePoints
        {
            get
            {
                if (CurrentZone?.Boundaries == null)
                    return null;

                return CurrentZone.Boundaries.Select(boundary => boundary.FromPosition).ToList();
            }
        }

        public Vector3 Position
        {
            get => _walker.CurrentPosition;
            set => _walker.CurrentPosition = value;
        }

        public List<Zone> ZonesToTravelThrough { get; set; } = new List<Zone>();

        public Zone CurrentZone { get; set; }

        public World World { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private void WalkerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPosition")
            {
                IWalker walker = (IWalker) sender;
                CurrentZone.Map.AddKnownNode(walker.CurrentPosition);
                // Teleport to the new zone if position is equal to a border zone position
            }
        }

        private void WalkerOnIsStuck(object sender, Vector3 e)
        {
            AddUnWalkableNodeAndGetNewPath(e);
        }

        private void AddUnWalkableNodeAndGetNewPath(Vector3 position)
        {
            CurrentZone.Map.AddUnWalkableNode(position);
            var path = Pathfinding.FindWaypoints(CurrentZone.Map, _walker.CurrentPosition, goalPosition);
            if (path == null)
            {
                _pathToWalk = new Queue<Vector3>();
                return;
            }

            _pathToWalk = new Queue<Vector3>();
            foreach (var vector3 in path) _pathToWalk.Enqueue(vector3);
        }

        public void PathfindAndWalkToFarAwayWorldMapPosition(Vector3 waypoint)
        {
            goalPosition = waypoint;

            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Position, waypoint);
            _pathToWalk = new Queue<Vector3>(path);
            while (_pathToWalk.Count > 0)
            {
                var point = _pathToWalk.Dequeue();
                GoToPosition(point);
            }
        }

        public void GoToPosition(Vector3 targetPosition)
        {
            _walker.WalkToPosition(targetPosition);
        }

        public ZoneBoundary GetZoneBorderToNameFromPoint(Vector3 position)
        {
            return CurrentZone.Boundaries.Find(b => b.FromPosition == position);
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
            if (nation == "SandOria")
                return World.Npcs.Find(n => n.Name.Contains("T.K."));
            if (nation == "Windurst")
                return World.Npcs.Find(n => n.Name.Contains("T.K."));

            throw new Exception("Can't find an NPC in global NPCs to give Signet to Nation: " + nation);
        }
    }
}