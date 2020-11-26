using System;
using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;

namespace Pathfinder
{
    public static class GridMath
    {
        public static Vector3 RoundVector3(Vector3 pos)
        {
            var x = ConvertFromFloatToInt(pos.X);
            var y = ConvertFromFloatToInt(pos.Y);
            var z = ConvertFromFloatToInt(pos.Z);

            return new Vector3(x, y, z);
        }

        public static int ConvertFromFloatToInt(float startingFloat)
        {
            var roundedInt = Convert.ToInt32(Math.Round(Convert.ToDouble(startingFloat)));
            return roundedInt;
        }


        /// <summary>
        ///     Clamp helps differentiate between positive and negative.
        ///     so if it is negative it sets it to 0, and if positive it sets it to 1.
        /// </summary>
        public static float Clamp(float val, float min, float max)
        {
            if (val.CompareTo(min) < 0) return min;
            if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Math.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Math.Abs(nodeA.GridY - nodeB.GridY);

            int dist;
            if (dstX > dstY)
            {
                dist = 14 * dstY + 10 * (dstX - dstY);
                return dist / 10;
            }

            dist = 14 * dstX + 10 * (dstY - dstX);
            return dist / 10;
        }


        public static int GetDistancePos(Vector3 start, Vector3 end)
        {
            int dstX = GridMath.ConvertFromFloatToInt(Math.Abs(start.X - end.X));
            int dstY = GridMath.ConvertFromFloatToInt(Math.Abs(start.Z - end.Z));

            int dist;
            if (dstX > dstY)
            {
                // multiplied by 10 for calcs, should be /10
                dist = 14 * dstY + 10 * (dstX - dstY);
                return dist / 10;
            }

            // multiplied by 10 for calcs, should be /10
            dist = 14 * dstX + 10 * (dstY - dstX);
            return dist / 10;
        }

        public static List<Vector3> GetPerpendicularPoints(Vector3 fromPoint, Vector3 toPoint,
            int PointsToAddToEachSide)
        {
            var differenceX = fromPoint.X - toPoint.X;
            var differenceZ = fromPoint.Z - toPoint.Z;

            var pointsAddedToEachSide = new List<Vector3>();

            if (differenceZ == 0 && differenceX == 0)
                return new List<Vector3>(); // No movement, no points to add


            if (differenceZ == 0)
            {
                for (int i = 1; i <= PointsToAddToEachSide; i++)
                {
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X, 0, toPoint.Z + i));
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X, 0, toPoint.Z - i));
                }

                return pointsAddedToEachSide;
            }

            if (differenceX == 0)
            {
                for (int i = 1; i <= PointsToAddToEachSide; i++)
                {
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X + i, 0, toPoint.Z));
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X - i, 0, toPoint.Z));
                }

                return pointsAddedToEachSide;
            }

            if (DifferenceXandYisOpposite(differenceX, differenceZ))
            {
                for (int i = 1; i <= PointsToAddToEachSide; i++)
                {
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X + i, 0, toPoint.Z + i));
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X - i, 0, toPoint.Z - i));
                }

                return pointsAddedToEachSide;
            }

            if (DifferenceXandYisSame(differenceX, differenceZ))
            {
                for (int i = 1; i <= PointsToAddToEachSide; i++)
                {
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X - i, 0, toPoint.Z + i));
                    pointsAddedToEachSide.Add(new Vector3(toPoint.X + i, 0, toPoint.Z - i));
                }

                return pointsAddedToEachSide;
            }


            return new List<Vector3>();
        }

        private static bool DifferenceXandYisOpposite(float differenceX, float differenceZ)
        {
            if (differenceX < 0 && differenceZ > 0)
                return true;

            if (differenceX > 0 && differenceZ < 0)
                return true;

            return false;
        }

        private static bool DifferenceXandYisSame(float differenceX, float differenceZ)
        {
            if (differenceX < 0 && differenceZ < 0)
                return true;

            if (differenceX > 0 && differenceZ > 0)
                return true;

            return false;
        }
    }
}