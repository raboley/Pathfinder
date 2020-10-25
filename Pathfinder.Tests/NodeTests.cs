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

           Assert.Equal(want, got);
           Assert.Equal(want.WorldPosition, got.WorldPosition);
        }
    }
}
