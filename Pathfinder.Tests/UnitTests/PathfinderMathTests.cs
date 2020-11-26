using System.Collections.Generic;
using System.Numerics;
using FluentAssertions;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class GridMathTests
    {
        [Fact]
        public void ConvertFromFloatToIntCanRoundAndConvertCorrectly()
        {
            var expected = 1;
            var startingFloat = 0.51f;

            int actual = GridMath.ConvertFromFloatToInt(startingFloat);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertFromFloatToIntRoundsDownWhenLessThanFive()
        {
            var expected = 0;
            var startingFloat = 0.4999f;

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
            var want = 0f;

            float got = GridMath.Clamp(number, 0, 1);

            Assert.Equal(want, got);
        }

        [Fact]
        public void ClampTurnsPositiveNumberToOne()
        {
            float number = 1;
            var want = 1f;

            float got = GridMath.Clamp(number, 0, 1);

            Assert.Equal(want, got);
        }


        [Fact]
        public void GetPerpendicularPointsCanGetThemForXAxis()
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|     |     |     |     |  z  |
-------------------------------
|     |     |     |     |  z  |
-------------------------------
|     |     | 0,0 |  t  |  z  |
-------------------------------
|     |     |     |     |  z  |
-------------------------------
|     |     |     |     |  z  |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(2, 0, 1),
                new Vector3(2, 0, 2),
                new Vector3(2, 0, -1),
                new Vector3(2, 0, -2)
            };


            var from = new Vector3(1, 0, 0);
            var to = new Vector3(2, 0, 0);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            want.Should().BeEquivalentTo(got);
        }

        [Fact]
        public void GetPerpendicularPointsCanGetThemForYAxis()
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|  z  |  z  |  z  |  z  |  z  |
-------------------------------
|     |     |  t  |     |     |
-------------------------------
|     |     | 0,0 |     |     |
-------------------------------
|     |     |     |     |     |
-------------------------------
|     |     |     |     |     |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(1, 0, 2),
                new Vector3(2, 0, 2),
                new Vector3(-1, 0, 2),
                new Vector3(-2, 0, 2)
            };


            var from = new Vector3(0, 0, 1);
            var to = new Vector3(0, 0, 2);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            got.Should().BeEquivalentTo(want);
        }

        [Fact]
        public void GetPerpendicularPointsCanGetThemForXAxisNegative()
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|  z  |     |     |     |     |
-------------------------------
|  z  |     |     |     |     |
-------------------------------
|  z  |  t  | 0,0 |     |     |
-------------------------------
|  z  |     |     |     |     |
-------------------------------
|  z  |     |     |     |     |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(-2, 0, 1),
                new Vector3(-2, 0, 2),
                new Vector3(-2, 0, -1),
                new Vector3(-2, 0, -2)
            };


            var from = new Vector3(-1, 0, 0);
            var to = new Vector3(-2, 0, 0);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            want.Should().BeEquivalentTo(got);
        }

        [Fact]
        public void GetPerpendicularPointsCanGetThemForYAxisNegative()
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|     |     |     |     |     |
-------------------------------
|     |     |     |     |     |
-------------------------------
|     |     | 0,0 |     |     |
-------------------------------
|     |     |  t  |     |     |
-------------------------------
|  z  |  z  |  z  |  z  |  z  |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(1, 0, -2),
                new Vector3(2, 0, -2),
                new Vector3(-1, 0, -2),
                new Vector3(-2, 0, -2)
            };


            var from = new Vector3(0, 0, -1);
            var to = new Vector3(0, 0, -2);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            got.Should().BeEquivalentTo(want);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        public void GetPerpendicularPointsCanDoDiagonalPoints(int fromX, int fromY)
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|     |     |     |     |  z  |
-------------------------------
|     |  t1 |     |  z  |     |
-------------------------------
|     |     |  z  |     |     |
-------------------------------
|     |  z  |     |  t2 |     |
-------------------------------
|  z  |     |     |     |     |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(-2, 0, -2),
                new Vector3(-1, 0, -1),
                new Vector3(2, 0, 2),
                new Vector3(1, 0, 1),
            };


            var from = new Vector3(fromX, 0, fromY);
            var to = new Vector3(0, 0, 0);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            got.Should().BeEquivalentTo(want);
        }


        [Theory]
        [InlineData(-1, -1)]
        [InlineData(1, 1)]
        public void GetPerpendicularPointsCanDoDiagonalPointsWithSameDiff(int fromX, int fromY)
        {
            /*
  * Visualization of the MapGrid.
  * t = traveler
  * z = zone border
-------------------------------
|  z  |     |     |     |     |
-------------------------------
|     |  z  |     |  t2 |     |
-------------------------------
|     |     |  z  |     |     |
-------------------------------
|     |  t1 |     |  z  |     |
-------------------------------
|     |     |     |     |  z  |
-------------------------------
  */
            var want = new List<Vector3>
            {
                new Vector3(-2, 0, 2),
                new Vector3(-1, 0, 1),
                new Vector3(1, 0, -1),
                new Vector3(2, 0, -2),
            };


            var from = new Vector3(fromX, 0, fromY);
            var to = new Vector3(0, 0, 0);

            List<Vector3> got = GridMath.GetPerpendicularPoints(from, to, 2);
            Assert.Equal(want.Count, got.Count);
            // Fluent Assertions don't care about order.
            got.Should().BeEquivalentTo(want);
        }
    }
}