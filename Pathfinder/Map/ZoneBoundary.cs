using System.Numerics;

namespace Pathfinder.Map
{
    public class ZoneBoundary
    {
        public string ToZone { get; set; }
        public string FromZone { get; set; }
        public Vector3 FromPosition { get; set; }
        public Vector3 ToPosition { get; set; }
    }
}