using Pathfinder.Pathfinder;
using Xunit;
using System.Numerics;

 namespace Pathfinder.Tests.Pathfinder
{
    public class NodeTests
    {
        [Fact]
        public void CanCreateNodeFromVector3()
        {
            // Arrange
            Vector3 position = new Vector3(1.0f);

            // Act
            GridNode gridNode= new GridNode(position);

            // Assert
            Assert.Equal(true, gridNode.walkable);
            Assert.Equal(position, gridNode.worldPosition);
        }
    }
}
