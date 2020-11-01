﻿using System;

namespace Pathfinder
{
    public static class GridMath
    {
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
    }
}