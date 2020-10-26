using System;
using System.Numerics;

namespace Pathfinder
{
    [Serializable]
    public class GridNode : IHeapItem<GridNode>
    {
        public bool Unknown { get; set; }
        public bool Walkable { get; set; }
        public int HeapIndex { get; set; }


        [NonSerialized] public int GCost;
        [NonSerialized] public int HCost;
        [NonSerialized] public GridNode Parent;

        public GridNode(Vector3 worldPos, bool _walkable = true, bool unknown = true)
        {
            Walkable = _walkable;
            WorldPosition = worldPos;
            Unknown = unknown;
        }


        public int FCost
        {
            get { return GCost + HCost; }
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


        public int CompareTo(GridNode gridNodeToCompare)
        {
            int compare = FCost.CompareTo(gridNodeToCompare.FCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(gridNodeToCompare.HCost);
            }

            return -compare;
        }

        public int GridX;
        public int GridY;

        public float X;
        public float Y;
        public float Z;
    }
}