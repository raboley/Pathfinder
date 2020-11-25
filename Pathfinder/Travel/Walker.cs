using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Pathfinder.Annotations;
using Pathfinder.Map;

namespace Pathfinder.Travel
{
    public class Walker : IWalker, INotifyPropertyChanged
    {
        private Vector3 _currentPosition;
        public Zone RevealedZone;


        public Walker(Vector3 currentPosition)
        {
            CurrentPosition = currentPosition;
        }

        public Zone CurrentZone { get; set; }
        public Queue<Vector3> PositionHistory { get; } = new Queue<Vector3>();

        public event EventHandler<Vector3> IsStuck;

        public bool Zoning { get; set; }

        public virtual void OnWalkerIsStuck(Vector3 currentPosition)
        {
            IsStuck?.Invoke(this, currentPosition);
        }

        public Vector3 CurrentPosition
        {
            get => _currentPosition;
            set
            {
                if (value.Equals(_currentPosition)) return;
                _currentPosition = value;
                PositionHistory.Enqueue(value);
                if (PositionHistory.Count >= 15) PositionHistory.Dequeue();
                OnPropertyChanged();
            }
        }

        public bool TryToWalkToPosition(Vector3 targetPosition)
        {
            var goal = targetPosition;
            int i = 0;

            while (CurrentPosition != goal && i < 50 && Zoning == false)
            {
                i++;

                int x = GetNewXorY(CurrentPosition.X, goal.X);
                int y = GetNewXorY(CurrentPosition.Z, goal.Z);

                var position = new Vector3(x, 0, y);
                // Try to move, if you can't move return fail?
                if (CantWalkToPosition(position))
                {
                    OnWalkerIsStuck(position);
                    return false;
                }

                CurrentPosition = position;
            }

            
            Zoning = false;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool CantWalkToPosition(Vector3 newPosition)
        {
            if (RevealedZone == null) return false;
            var node = RevealedZone.Map.GetNodeFromWorldPoint(newPosition);
            return !node.Walkable;
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}