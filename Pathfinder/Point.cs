using System.Numerics;

namespace Pathfinder
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public static Point Zero => new Point(0, 0, 0);
        public static Point One => new Point(1, 1, 1);

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point(float x, float y, float z)
        {
            X = GridMath.ConvertFromFloatToInt(x);
            Y = GridMath.ConvertFromFloatToInt(y);
            Z = GridMath.ConvertFromFloatToInt(z);
        }

        public Point(Vector3 vector3)
        {
            X = GridMath.ConvertFromFloatToInt(vector3.X);
            Y = GridMath.ConvertFromFloatToInt(vector3.Y);
            Z = GridMath.ConvertFromFloatToInt(vector3.Z);
        }
    }
}