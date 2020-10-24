﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Pathfinder.Pathfinder;

namespace Pathfinder.Pathfinder
{
    public class GridNode : IHeapItem<GridNode>
    {
        public bool Walkable;
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

        public int CompareTo(GridNode gridNodeToCompare)
        {
            int compare = fCost.CompareTo(gridNodeToCompare.fCost);
            if (compare == 0)
            {
                compare = HCost.CompareTo(gridNodeToCompare.HCost);
            }

            return -compare;
        }
    }
}