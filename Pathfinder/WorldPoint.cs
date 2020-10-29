using System.Numerics;

namespace Pathfinder
{
    public struct WorldPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public WorldPoint(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public WorldPoint(float x, float y, float z)
        {
            X = GridMath.ConvertFromFloatToInt(x);
            Y = GridMath.ConvertFromFloatToInt(y);
            Z = GridMath.ConvertFromFloatToInt(z);
        }

        public WorldPoint(Vector3 vector3)
        {
            X = GridMath.ConvertFromFloatToInt(vector3.X);
            Y = GridMath.ConvertFromFloatToInt(vector3.Y);
            Z = GridMath.ConvertFromFloatToInt(vector3.Z);
        }
    }
}