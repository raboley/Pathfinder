using System.Numerics;
using Pathfinder.Persistence;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WorldMapNodeTests
    {
        [Fact]
        public void CanCreateWorldMapNodeFromVector3()
        {
            // Arrange
            var position = new Vector3(1.0f);

            // Act
            var gridNode = new WorldMapNode(position);

            // Assert
            Assert.Equal(true, gridNode.Walkable);
            Assert.Equal(position, gridNode.WorldPosition);
        }

        [Fact]
        public void TestCanSaveToFile()
        {
            var want = new WorldMapNode(Vector3.One);
            var persister = new FilePersister("tempNode.golden");

            persister.Save(want);

            var got = persister.Load<WorldMapNode>();
            persister.Delete();

            Assert.Equal(want.WorldPosition, got.WorldPosition);
        }
    }
}