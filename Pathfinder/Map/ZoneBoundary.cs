using System;
using System.Numerics;

namespace Pathfinder.Map
{
    public class ZoneBoundary : IEquatable<ZoneBoundary>
    {
        public string ToZone { get; set; }
        public string FromZone { get; set; }
        public Vector3 FromPosition { get; set; }
        public Vector3 ToPosition { get; set; }

        public bool Equals(ZoneBoundary other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ToZone == other.ToZone && FromZone == other.FromZone && FromPosition.Equals(other.FromPosition) &&
                   ToPosition.Equals(other.ToPosition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ZoneBoundary) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (ToZone != null ? ToZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FromZone != null ? FromZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FromPosition.GetHashCode();
                hashCode = (hashCode * 397) ^ ToPosition.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ZoneBoundary left, ZoneBoundary right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ZoneBoundary left, ZoneBoundary right)
        {
            return !Equals(left, right);
        }
    }
}