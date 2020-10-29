using System;
using System.Numerics;

namespace Pathfinder.WorldMap
{
    [Serializable]
    public class WorldMapNode : IHeapItem<WorldMapNode>
    {
        [NonSerialized] public int GCost;

        public int GridX;
        public int GridY;
        [NonSerialized] public int HCost;
        [NonSerialized] public WorldMapNode Parent;

        public float X;
        public float Y;
        public float Z;

        public WorldMapNode(Vector3 worldPos, bool _walkable = true, bool unknown = true)
        {
            Walkable = _walkable;
            WorldPosition = worldPos;
            Unknown = unknown;
        }

        public bool Unknown { get; set; }
        public bool Walkable { get; set; }


        public int FCost => GCost + HCost;

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

        public int HeapIndex { get; set; }


        public int CompareTo(WorldMapNode worldMapNodeToCompare)
        {
            int compare = FCost.CompareTo(worldMapNodeToCompare.FCost);
            if (compare == 0) compare = HCost.CompareTo(worldMapNodeToCompare.HCost);

            return -compare;
        }
    }
}