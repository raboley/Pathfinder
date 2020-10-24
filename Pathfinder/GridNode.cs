﻿using System;
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
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public int gCost;
        public int hCost;
        public GridNode parent;
        int heapIndex;

        public GridNode(Vector3 _worldPos, bool _walkable = true)
        {
            walkable = _walkable;
            worldPosition = _worldPos;
        }

        public int fCost
        {
            get { return gCost + hCost; }
        }

        public int HeapIndex
        {
            get { return heapIndex; }
            set { heapIndex = value; }
        }

        public int CompareTo(GridNode gridNodeToCompare)
        {
            int compare = fCost.CompareTo(gridNodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(gridNodeToCompare.hCost);
            }

            return -compare;
        }
    }
}
