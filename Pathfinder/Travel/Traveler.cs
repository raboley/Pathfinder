using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
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
        private bool _zoning;
        public ZoneMap BlindGrid;
        public Vector3 GoalPosition;
        public Zone GoalZone;
        public Queue<Vector3> PathToWalk;

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

        public bool IsDead { get; set; } = false;

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

        public ConcurrentQueue<Zone> ZonesToTravelThrough { get; set; } = new ConcurrentQueue<Zone>();

        public Zone CurrentZone { get; set; }

        public World World { get; set; }

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

        public bool MenuIsOpen { get; set; } = false;

        public bool IsFighting { get; set; } = false;


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
            if (IsDead)
                return;

            // Also don't record stuck if menu is open.
            if (!IsFighting && !MenuIsOpen)
                CurrentZone.Map.AddUnWalkableNode(position);

            IPathfindingStyle style = null;
            if (distanceTolerance > 0)
                style = new CloseEnoughStyle() {DistanceTolerance = distanceTolerance};

            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Walker.CurrentPosition, GoalPosition, style);
            if (path == null)
            {
                PathToWalk = new Queue<Vector3>();
                return;
            }

            PathToWalk = new Queue<Vector3>();
            foreach (var vector3 in path) PathToWalk.Enqueue(vector3);
        }

        public void PathfindAndWalkToFarAwayWorldMapPosition(Vector3 waypoint, int distanceTolerance = 0,
            int secondsToRunFor = 5)
        {
            GoalPosition = waypoint;

            IPathfindingStyle style = null;
            if (distanceTolerance > 0)
                style = new CloseEnoughStyle() {DistanceTolerance = distanceTolerance};

            var path = Pathfinding.FindWaypoints(CurrentZone.Map, Position, waypoint, style);
            if (path == null)
            {
                Walker.TryToEscapeFromBeingInABoxOfUnWalkablePositions();
                return;
            }

            PathToWalk = new Queue<Vector3>(path);

            // TODO: Maybe update this to be better....
            // Either have the engine break out of traveling on events, or pass in a closure of all conditions.
            DateTime duration = DateTime.Now.AddSeconds(secondsToRunFor);
            while (PathToWalk.Count > 0 && Zoning == false && IsDead == false && DateTime.Now < duration
                   && GridMath.GetDistancePos(Walker.CurrentPosition, waypoint) >= distanceTolerance
            )
            {
                var point = PathToWalk.Dequeue();
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


        private void GoToZone(string zone)
        {
            if (Zoning)
                return;
            // Already in that zone!
            if (CurrentZone.Name == zone)
            {
                return;
            }

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

        public void WalkToZone(string zoneName, bool nonStop = false)
        {
            var zonesToTravelTo = WorldPathfinder.FindWorldPathToZone(World, CurrentZone.Name, zoneName);
            if (zonesToTravelTo == null)
                throw new Exception("Couldn't find linking zones from: " + CurrentZone.Name + " to zone:" + zoneName);

            if (nonStop)
            {
                while (CurrentZone.Name != zoneName)
                {
                    WalkThroughZones(zonesToTravelTo);
                }
            }
            else
            {
                WalkThroughZones(zonesToTravelTo);
            }
        }

        private void WalkThroughZones(List<Zone> zonesToTravelTo)
        {
            if (CurrentZone.Name == zonesToTravelTo[0].Name)
                zonesToTravelTo.RemoveAt(0);

            if (zonesToTravelTo.Count == 0)
                return;
            
            GoToZone(zonesToTravelTo[0].Name);

            // Since traveler exits, we can't expect to be at the next zone by the time the for each loop goes.
            // foreach (var zone in zonesToTravelTo)
            // {
            //     GoToZone(zone.Name);
            // }
        }

        public Person SearchForClosestSignetNpc(string nation, List<Person> allPeople)
        {
            var npcPatterns = GetSignetNpcPatternByNation(nation);
            return GetClosestMatchingNpcInAnyZoneByPatterns(allPeople, npcPatterns);
        }

        private Person GetClosestMatchingNpcInAnyZoneByPatterns(List<Person> allPeople, List<string> npcPatterns)
        {
            // No Matches in current zone
            var npcs = FindAllNpcsMatchingPatternsInWorld(npcPatterns, allPeople);

            // find all in the same zone.
            var signetPeopleInSameZone = npcs.FindAll(x => x.MapName == CurrentZone.Name);
            if (signetPeopleInSameZone.Count > 0)
            {
                return GetClosestPerson(signetPeopleInSameZone);
            }

            return GetClosestPersonInAnyZone(npcs);
        }

        private Person GetClosestPersonInAnyZone(List<Person> npcs)
        {
            int shortestZoneChangeCount = 99999;
            Person closestPerson = null;
            foreach (var npc in npcs)
            {
                var zonesToTravelThrough = WorldPathfinder.FindWorldPathToZone(World, CurrentZone.Name, npc.MapName);
                if (zonesToTravelThrough == null)
                    continue;
                
                if (zonesToTravelThrough.Count < shortestZoneChangeCount)
                {
                    shortestZoneChangeCount = zonesToTravelThrough.Count;
                    closestPerson = npc;
                }
            }

            return closestPerson;
        }

        private Person GetClosestPerson(List<Person> signetPeopleInSameZone)
        {
            var currentShortestDistance = 9999;
            Person closestSignetPerson = null;
            foreach (var person in signetPeopleInSameZone)
            {
                var distance = GridMath.GetDistancePos(Walker.CurrentPosition, person.Position);
                if (distance < currentShortestDistance)
                {
                    currentShortestDistance = distance;
                    closestSignetPerson = person;
                }
            }

            return closestSignetPerson;
        }

        private List<Person> FindAllNpcsMatchingPatternsInWorld(List<string> npcPatterns, List<Person> npcs)
        {
            var signetNpcs = new List<Person>();
            foreach (var npcPattern in npcPatterns)
            {
                signetNpcs.AddRange(npcs.FindAll(n => n.Name.Contains(npcPattern)));
            }

            return signetNpcs;
        }

        public List<string> GetSignetNpcPatternByNation(string nation)
        {
            var nationStrings = new List<string>();
            if (nation == "Bastok")
            {
                nationStrings.Add("I.M.");
                return nationStrings;
            }

            if (nation == "SandOria")
            {
                nationStrings.Add("R.K");
                nationStrings.Add("T.K");
                return nationStrings;
            }
            // if (nation == "Windurst")
            //     return "T.K.";

            throw new Exception("Don't know the NPC pattern for nation: " + nation);
        }

        public void WalkAcrossWorldToNpcByName(string npcName)
        {
            var npc = GetClosestMatchingNpcInAnyZoneByName(npcName);
            WalkAcrossTheWorldToPerson(npc);
        }

        private Person GetClosestMatchingNpcInAnyZoneByName(string npcName)
        {
            var allPeople = World.GetAllNpcs();
            var names = new List<string> {npcName};
            var npc = GetClosestMatchingNpcInAnyZoneByPatterns(allPeople, names);
            return npc;
        }

        private void WalkAcrossTheWorldToPerson(Person npc)
        {
            if (CurrentZone.Name != npc.MapName)
            {
                WalkToZone(npc.MapName);
            }

            if (CurrentZone.Name == npc.MapName)
            {
                PathfindAndWalkToFarAwayWorldMapPosition(npc.Position);
            }
        }

        public int GetDistanceFromPersonByName(string gateGuardName)
        {
            var npc = GetClosestMatchingNpcInAnyZoneByName(gateGuardName);
            if (CurrentZone.Name != npc.MapName)
                return Int32.MaxValue;

            return GridMath.GetDistancePos(Walker.CurrentPosition, npc.Position);
        }

        public bool AmNotWithinTalkingDistanceToPersonByName(string name)
        {
            int distance = GetDistanceFromPersonByName(name);
            if (distance > 1)
                return true;

            return false;
        }
    }
}