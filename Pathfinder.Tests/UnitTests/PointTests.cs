using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PointTests
    {
        [Fact]
        public void DefaultConstructorCreatesTypeWithAllPropsAccessible()
        {
            const int wantX = 1;
            const int wantY = 2;
            const int wantZ = 3;

            var got = new Point(1, 2, 3);

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }

        [Fact]
        public void FloatConstructorRoundsCorrectly()
        {
            const int wantX = 1;
            const int wantY = 2;
            const int wantZ = 3;

            var got = new Point(1.01f, 1.50f, 3.49f);

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }


        [Fact]
        public void VectorConstructorRoundsAndConvertsCorrectly()
        {
            const int wantX = 1;
            const int wantY = -2;
            const int wantZ = 3;

            var got = new Point(new Vector3(1.01f, -1.50f, 3.49f));

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }

        [Fact]
        public void ZeroCreatesAnAllZeroPoint()
        {
            const int wantX = 0;
            const int wantY = 0;
            const int wantZ = 0;

            var got = Point.Zero;

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }

        [Fact]
        public void OneCreatesAnAllOneWorldPoint()
        {
            const int wantX = 1;
            const int wantY = 1;
            const int wantZ = 1;

            var got = Point.One;

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }
    }
}