﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Pathfinder.Pathfinder;

namespace Pathfinder.Pathfinder
{
    [Serializable]
    public class GridNode : IHeapItem<GridNode>
    {
        public bool Walkable;
        
        [NonSerialized]
        public Vector3 WorldPosition;
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

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
    }
}
