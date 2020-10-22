using System.Collections.Generic;
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
            
            // Act
            Grid grid = new Grid(Vector2.One);
            grid.CreateGrid();
            Node[,] got = grid.grid;
            
            // Assert
            Assert.Equal(want.Length, got.Length);
            AssertGridEqual(want, got);
        }

        [Fact]
        public void CanCreateGridWithMultipleNodes()
        {
            // Arrange
            Node[,] want = SetupThreeByThreeGrid();

            // Act
            Grid grid = new Grid(new Vector2(3f, 3f));
            grid.CreateGrid();
            Node[,] got = grid.grid;
            
            // Assert
            Assert.Equal(want.Length, got.Length);
            AssertGridEqual(want, got); 
        }

        [Fact]
        public void GetNeighboursCanGetNeighborsFromEdgeNode()
        {
            //Arrange
            var want = GetNeighborsListForEdgeNode();

            //Act
            Grid grid = new Grid(new Vector2(3f, 3f));
            grid.CreateGrid();
            List<Node> got = grid.GetNeighbours(grid.grid[2, 2]);
            
            //Assert
            AssertListEqual(want, got);
        }

        [Fact]
        public void GetNodeFromWorldPoint()
        {
            //Arrange
            Node want = new Node((Vector3.One));
            
            //Act
            Grid grid = new Grid(new Vector2(3f, 3f));
            grid.CreateGrid();
            Node got = grid.NodeFromWorldPoint(new Vector3(.99f, 1f, .5f));

            //Assert
            Assert.Equal(want, got);
        }

        private static List<Node> GetNeighborsListForEdgeNode()
        {
            List<Node> want = new List<Node>();
            want.Add(new Node(new Vector3(-1, 0, -1)));
            want.Add(new Node(new Vector3(-1, 0, 0)));
            want.Add(new Node(new Vector3(-1, 0, 1)));
            want.Add(new Node(new Vector3(0, 0, -1)));
            want.Add(new Node(new Vector3(0, 0, 1)));
            want.Add(new Node(new Vector3(1, 0, -1)));
            want.Add(new Node(new Vector3(1, 0, 0)));
            want.Add(new Node(new Vector3(1, 0, 1)));
            return want;
        }

        private static void AssertListEqual(List<Node> want, List<Node> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (int i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i], got[i]);
            }
        }

        private static Node[,] SetupThreeByThreeGrid()
        {
            Node[,] want = new Node[3, 3];
            want[0, 0] = new Node(new Vector3(-1f, 0, -1f));
            want[0, 1] = new Node(new Vector3(-1f, 0f, 0f));
            want[0, 2] = new Node(new Vector3(-1f, 0f, 1f));
            want[1, 0] = new Node(new Vector3(0f, 0f, -1f));
            want[1, 1] = new Node(new Vector3(0f, 0f, 0f));
            want[1, 2] = new Node(new Vector3(0f, 0f, 1f));
            want[2, 0] = new Node(new Vector3(1f, 0f, -1f));
            want[2, 1] = new Node(new Vector3(1f, 0f, 0f));
            want[2, 2] = new Node(new Vector3(1f, 0f, 1f));
            return want;
        }


        private static void AssertGridEqual(Node[,] want, Node[,] got)
        {
            for (int i = 0; i <= want.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= want.GetUpperBound(1); j++)
                {
                    AssertNodeFromGridsEqual(want, got, i, j);
                }
            }
        }

        private static void AssertNodeFromGridsEqual(Node[,] want, Node[,] got, int i, int j)
        {
            Assert.Equal(want[i, j].worldPosition, got[i, j].worldPosition);
            Assert.Equal(want[i, j].walkable, got[i, j].walkable);
        }
    }
}
