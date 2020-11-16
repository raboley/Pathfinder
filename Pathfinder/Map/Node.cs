using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pathfinder.Map
{
    public class NodeConverter : JsonConverter<Node>
    {
        public override void WriteJson(JsonWriter writer, Node value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject) t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override Node ReadJson(JsonReader reader, Type objectType, Node existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
            // string s = (string)reader.Value;
            //
            // return new Node(s);
        }
    }

    [Serializable]
    public class Node : IHeapItem<Node>, IEquatable<Node>
    {
        [NonSerialized] public int GCost;

        [NonSerialized] public int GridX;
        [NonSerialized] public int GridY;
        [NonSerialized] public int HCost;
        [NonSerialized] public Node Parent;

        public int X;
        public int Y;
        public int Z;

        public Node(Vector3 worldPos, bool _walkable = true, bool unknown = true)
        {
            Walkable = _walkable;
            WorldPosition = worldPos;
            Unknown = unknown;
        }

        public bool Unknown { get; set; }
        public bool Walkable { get; set; }


        [JsonIgnore] public int FCost => GCost + HCost;

        [JsonIgnore]
        public Vector3 WorldPosition
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = GridMath.ConvertFromFloatToInt(value.X);
                Y = GridMath.ConvertFromFloatToInt(value.Y);
                Z = GridMath.ConvertFromFloatToInt(value.Z);
            }
        }

        public bool Equals(Node other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z) && Unknown == other.Unknown &&
                   Walkable == other.Walkable;
        }

        [JsonIgnore] public int HeapIndex { get; set; }


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