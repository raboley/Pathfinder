using System;
using System.Numerics;

namespace Pathfinder.Map.WorldMap
{
    [Serializable]
    public class Node : IHeapItem<Node>, IEquatable<Node>
    {
        [NonSerialized] public int GCost;

        public int GridX;
        public int GridY;
        [NonSerialized] public int HCost;
        [NonSerialized] public Node Parent;

        public float X;
        public float Y;
        public float Z;

        public Node(Vector3 worldPos, bool _walkable = true, bool unknown = true)
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

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && Unknown == other.Unknown &&
                   Walkable == other.Walkable;
        }

        public int HeapIndex { get; set; }


        public int CompareTo(Node nodeToCompare)
        {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0) compare = HCost.CompareTo(nodeToCompare.HCost);

            return -compare;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = GridX;
                hashCode = (hashCode * 397) ^ GridY;
                hashCode = (hashCode * 397) ^ X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                hashCode = (hashCode * 397) ^ Unknown.GetHashCode();
                hashCode = (hashCode * 397) ^ Walkable.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Node left, Node right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Node left, Node right)
        {
            return !Equals(left, right);
        }
    }
}