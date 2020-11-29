using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Pathfinder.Travel
{
    public interface IWalker
    {
        Vector3 CurrentPosition { get; set; }
        bool Zoning { get; set; }
        Queue<Vector3> PositionHistory { get; }
        bool TryToWalkToPosition(Vector3 targetPosition);
        void OnWalkerIsStuck(Vector3 currentPosition);
        event EventHandler<Vector3> IsStuck;
        event PropertyChangedEventHandler PropertyChanged;
        void TryToEscapeFromBeingInABoxOfUnWalkablePositions();
    }
}