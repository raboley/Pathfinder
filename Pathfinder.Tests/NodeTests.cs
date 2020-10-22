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
            Node node= new Node(position);

            // Assert
            Assert.Equal(true, node.walkable);
            Assert.Equal(position, node.worldPosition);
        }
    }
}
