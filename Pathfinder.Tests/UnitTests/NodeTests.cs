using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NodeTests
    {
        [Fact]
        public void CanCreateNodeFromVector3()
        {
            // Arrange
            var position = new Vector3(1.0f);

            // Act
            var gridNode = new GridNode(position);

            // Assert
            Assert.Equal(true, gridNode.Walkable);
            Assert.Equal(position, gridNode.WorldPosition);
        }

        [Fact]
        public void TestCanSaveToFile()
        {
            var want = new GridNode(Vector3.One);
            var persister = new FilePersister("tempNode.golden");

            persister.Save(want);

            var got = persister.Load<GridNode>();
            persister.Delete();

            Assert.Equal(want.WorldPosition, got.WorldPosition);
        }
    }
}