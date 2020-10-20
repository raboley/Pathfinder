using Pathfinder.Pathfinder;
using Xunit;
using System.Numerics;

 namespace Pathfinder.Tests.Pathfinder
{
    
    public class GridTests
    {
        [Fact]
        public void CreateGridCanGenerateGridFromCorner()
        {
            // Arrange
            Node[,] want = new Node[1, 1];
            want[0, 0] = new Node(Vector3.Zero);
            // want[1, 0] = new Node(new Vector3(1f, 0f, 0f));
            // want[1, 1] = new Node(new Vector3(1f, 0f, 1f));
            // want[1, -1] = new Node(new Vector3(1f, 0f, -1f));
            // want[0, 1] = new Node(new Vector3(0f, 0f, 1f));
            // want[0, -1] = new Node(new Vector3(0f, 0f, -1f));
            // want[-1, 0] = new Node(new Vector3(-1f, 0f, 0f));
            // want[-1, 1] = new Node(new Vector3(-1f, 0f, 1f));
            // want[-1, -1] = new Node(new Vector3(-1f, 0f, -1f));
            
            Grid grid = new Grid(Vector2.One);
            

            // Act
            grid.CreateGrid();
            Node[,] got = grid.grid;
            
            // Assert
            Assert.Equal(want.Length, got.Length);
            AssertGridEqual(want, got);
        }

        private static void AssertGridEqual(Node[,] want, Node[,] got)
        {
            for (int i = 0; i < want.Length; i++)
            {
                for (int j = 0; j < want.Length; j++)
                {
                    Assert.Equal(want[i, j], got[i, j]);
                }
            }
        }
    }
}
