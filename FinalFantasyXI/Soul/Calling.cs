using System;

namespace FinalFantasyXI.Soul
{
    public class Calling
    {
        public string Name { get; set; }
    }

    public class Objective : IEquatable<Objective>
    {
        public string Name { get; set; }

        public bool Equals(Objective other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Objective) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(Objective left, Objective right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Objective left, Objective right)
        {
            return !Equals(left, right);
        }
    }

    // Calling is Junior  Woodworker
    // Done = woodworkingSkill = 100;
    public class CallingFactory
    {
    }
}