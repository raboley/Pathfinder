using System.Numerics;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NodeTests
    {
        [Fact]
        public void CanCreateNodeFromPoint()
        {
            var position = Vector3.One;

            var gridNode = new Node(position);

            Assert.Equal(true, gridNode.Walkable);
            Assert.Equal(position, gridNode.WorldPosition);
        }
    }
}