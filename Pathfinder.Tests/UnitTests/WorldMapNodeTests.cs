using System.Numerics;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldMapNodeTests
    {
        [Fact]
        public void CanCreateWorldMapNodeFromVector3()
        {
            var position = new Vector3(1.0f);

            var gridNode = new WorldMapNode(position);

            Assert.Equal(true, gridNode.Walkable);
            Assert.Equal(position, gridNode.WorldPosition);
        }
    }
}