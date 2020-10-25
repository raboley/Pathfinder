using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class GridMathTests
    {
        [Fact]
        public void ConvertFromFloatToIntCanRoundAndConvertCorrectly()
        {
            int expected = 1;
            float startingFloat = 0.51f;

            int actual = GridMath.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntRoundsDownWhenLessThanFive()
        {
            int expected = 0;
            float startingFloat = 0.4999f;

            int actual = GridMath.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntCanRoundAndConvertNegativeCorrectly()
        {
            int expected = -1;
            float startingFloat = -0.51f;

            int actual = GridMath.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntRoundsDownWhenLessThanFiveNegative()
        {
            int expected = -0;
            float startingFloat = -0.4999f;

            int actual = GridMath.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ClampTurnsNegativeNumberToZero()
        {
            float number = -1;
            float want = 0f;

            float got = GridMath.Clamp(number, 0, 1);

            Assert.Equal(want, got);
        }

        [Fact]
        public void ClampTurnsPositiveNumberToOne()
        {
            float number = 1;
            float want = 1f;

            float got = GridMath.Clamp(number, 0, 1);

            Assert.Equal(want, got);
        }
    }
}