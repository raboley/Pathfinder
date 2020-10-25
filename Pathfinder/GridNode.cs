using System;
using System.Collections.Generic;
using System.Numerics;

namespace Pathfinder
{
    [Serializable]
    public class GridNode : IHeapItem<GridNode>
    {
        public bool Walkable;

        public int GridX;
        public int GridY;

        public int GCost;
        public int HCost;
        public GridNode Parent;
        private int _heapIndex;

        public GridNode(Vector3 _worldPos, bool _walkable = true)
        {
            Walkable = _walkable;
            WorldPosition = _worldPos;
            X = _worldPos.X;
            Y = _worldPos.Y;
            Z = _worldPos.Z;

            Entities = new List<IEntity>();
        }

        public Vector3 WorldPosition
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public int fCost
        {
            get { return GCost + HCost; }
        }

        public int HeapIndex
        {
            get { return _heapIndex; }
            set { _heapIndex = value; }
        }

        public bool Unknown { get; set; } = true;
        public List<IEntity> Entities { get; set; }

        public int CompareTo(GridNode gridNodeToCompare)
        {
            int compare = fCost.CompareTo(gridNodeToCompare.fCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(gridNodeToCompare.HCost);
            }

            return -compare;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}