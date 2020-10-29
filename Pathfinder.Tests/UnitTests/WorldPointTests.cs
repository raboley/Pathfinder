using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldPointTests
    {
        [Fact]
        public void DefaultConstructorCreatesTypeWithAllPropsAccessible()
        {
            const int wantX = 1;
            const int wantY = 2;
            const int wantZ = 3;

            var got = new WorldPoint(1, 2, 3);

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

            var got = new WorldPoint(1.01f, 1.50f, 3.49f);

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

            var got = new WorldPoint(new Vector3(1.01f, -1.50f, 3.49f));

            Assert.Equal(wantX, got.X);
            Assert.Equal(wantY, got.Y);
            Assert.Equal(wantZ, got.Z);
        }
    }
}