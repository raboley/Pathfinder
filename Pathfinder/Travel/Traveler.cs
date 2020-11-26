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
        public readonly IWalker Walker;
        public Queue<Vector3> _pathToWalk;
        public ZoneMap BlindGrid;
        public Vector3 goalPosition;
        private bool _zoning;
        public bool IsDead { get; set; } = false;

        public Traveler()
        {
            Walker = new Walker(Vector3.Zero);
            Walker.IsStuck += WalkerOnIsStuck;
            Walker.PropertyChanged += WalkerOnPropertyChanged;
        }

        public Traveler(string currentZoneName, World world, IWalker walker)
        {
            Walker = walker;
            Walker.IsStuck += WalkerOnIsStuck;
            Walker.PropertyChanged += WalkerOnPropertyChanged;
            World = world;
            CurrentZone = world.GetZoneByName(currentZoneName);
            // Position = walker.CurrentPosition;
            CurrentZone.Map.AddKnownNode(walker.CurrentPosition);
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
            get => Walker.CurrentPosition;
            set => Walker.CurrentPosition = value;
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

        private void AddUnWalkableNodeAndGetNewPath(Vector3 position, int distanceTolerance = 0)
        {
            CurrentZone.Map.AddUnWalkableNode(position);
            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Walker.CurrentPosition, goalPosition, distanceTolerance);
            if (path == null)
            {
                _pathToWalk = new Queue<Vector3>();
                return;
            }

            _pathToWalk = new Queue<Vector3>();
            foreach (var vector3 in path) _pathToWalk.Enqueue(vector3);
        }

        public void PathfindAndWalkToFarAwayWorldMapPosition(Vector3 waypoint, int distanceTolerance = 0)
        {
            goalPosition = waypoint;

            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Position, waypoint, distanceTolerance);
            if (path == null)
            {
                return;
            }

            _pathToWalk = new Queue<Vector3>(path);

            // TODO: Maybe update this to be better....
            // Either have the engine break out of traveling on events, or pass in a closure of all conditions.
            DateTime duration = DateTime.Now.AddSeconds(5);
            while (_pathToWalk.Count > 0 && Zoning == false && IsDead == false && DateTime.Now < duration && GridMath.GetDistancePos(Walker.CurrentPosition, waypoint) >= distanceTolerance)
            {
                var point = _pathToWalk.Dequeue();
                bool gotThere = GoToPosition(point);
                if (gotThere == false)
                    break;
            }
        }


        private bool GoToPosition(Vector3 targetPosition)
        {
            return Walker.TryToWalkToPosition(targetPosition);
        }

        public ZoneBoundary GetZoneBorderToNameFromPoint(Vector3 position)
        {
            return CurrentZone.Boundaries.Find(b => b.FromPosition == position);
        }


        public void GoToZone(string zone)
        {
            if (Zoning)
                return;
            // Already in that zone!
            if (CurrentZone.Name == zone)
                return;

            var pos = GetBorderZonePosition(zone);
            if (pos == null)
                throw new KeyNotFoundException("Don't know where zone: " + zone + " is.");
            
            PathfindAndWalkToFarAwayWorldMapPosition((Vector3) pos);
        }

        public Vector3? GetBorderZonePosition(string zone)
        {
            var borderZones = CurrentZone.Boundaries.FindAll(b => b.ToZone == zone);
            // Debug.Assert(borderZones != null, nameof(borderZones) + " != null");
            return GetClosestZoneBorder(borderZones);
        }

        public Vector3? GetClosestZoneBorder(List<ZoneBoundary> borderZones)
        {
            ZoneBoundary first = null;
            foreach (var zone in borderZones)
            {
                first = zone;
                if (first != null)
                    return first.FromPosition;
            }

            // Debug.Assert(first != null, nameof(first) + " != null");
            return null;
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
            string npcPattern = GetSignetNpcPatternByNation(nation);
            return World.Npcs.Find(n => n.Name.Contains(npcPattern));
        }

        public string GetSignetNpcPatternByNation(string nation)
        {
            if (nation == "Bastok")
                return "I.M.";
            if (nation == "SandOria")
                return "T.K.";
            // if (nation == "Windurst")
            //     return "T.K.";

            throw new Exception("Don't know the NPC pattern for nation: " + nation);
        }

        public bool Zoning
        {
            get => _zoning;
            set
            {
                // if(value.Equals(true))
                //     OnIsZoning();
                _zoning = value;
            }
        }
    }
}