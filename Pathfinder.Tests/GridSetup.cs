using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace Pathfinder.Tests
{
    public class GridSetup
    {
        public static string _smallGridZoneName = "Small Grid";

        public static void AssertPointNotWalkable(Grid grid, Vector3 position)
        {
            var node = grid.GetNodeFromWorldPoint(position);
            Assert.Equal(node.WorldPosition, position);
            Assert.Equal(node.Walkable, false);
        }

        public static Pathfinding SetupForPathfinding()
        {
            var pathfinding = new Pathfinding();
            var grid = Grid.NewGridFromVector2(new Vector2(5, 5));
            pathfinding.Grid = grid;
            return pathfinding;
        }

        public static Grid SetupSmallGrid()
        {
            var grid = Grid.NewGridFromVector2(new Vector2(3f, 3f));
            grid.MapName = _smallGridZoneName;
            return grid;
        }

        public static Grid SetupMediumGrid()
        {
            var grid = Grid.NewGridFromVector2(new Vector2(5f, 5f));
            return grid;
        }

        public static Grid SetupBigGrid()
        {
            var grid = Grid.NewGridFromVector2(new Vector2(51f, 51f));
            return grid;
        }

        public static List<GridNode> GetNeighborsListForEdgeNode()
        {
            var want = new List<GridNode>
            {
                new GridNode(new Vector3(0, 0, 0)),
                new GridNode(new Vector3(0, 0, 1)),
                new GridNode(new Vector3(1, 0, 0))
            };
            // want.Add(new GridNode(new Vector3(-1, 0, -1)));
            // want.Add(new GridNode(new Vector3(-1, 0, 0)));
            // want.Add(new GridNode(new Vector3(-1, 0, 1)));
            // want.Add(new GridNode(new Vector3(0, 0, -1)));
            // want.Add(new GridNode(new Vector3(0, 0, 1)));
            // want.Add(new GridNode(new Vector3(1, 0, -1)));
            // want.Add(new GridNode(new Vector3(1, 0, 0)));
            // want.Add(new GridNode(new Vector3(1, 0, 1)));
            return want;
        }

        public static void AssertListGridNodesEqual(IReadOnlyList<GridNode> want, List<GridNode> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (var i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i], got[i]);
            }
        }

        public static void AssertListEntitiesEqual(List<NPC> want, List<NPC> got)
        {
            Assert.Equal(want.Count, got.Count);

            for (var i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i].Name, got[i].Name);
                Assert.Equal(want[i].Position, got[i].Position);
                Assert.Equal(want[i].MapName, got[i].MapName);
            }
        }

        public static GridNode[,] SetupThreeByThreeGrid()
        {
            var want = new GridNode[3, 3];
            want[0, 0] = new GridNode(new Vector3(-1f, 0, -1f));
            want[0, 1] = new GridNode(new Vector3(-1f, 0f, 0f));
            want[0, 2] = new GridNode(new Vector3(-1f, 0f, 1f));
            want[1, 0] = new GridNode(new Vector3(0f, 0f, -1f));
            want[1, 1] = new GridNode(new Vector3(0f, 0f, 0f));
            want[1, 2] = new GridNode(new Vector3(0f, 0f, 1f));
            want[2, 0] = new GridNode(new Vector3(1f, 0f, -1f));
            want[2, 1] = new GridNode(new Vector3(1f, 0f, 0f));
            want[2, 2] = new GridNode(new Vector3(1f, 0f, 1f));
            return want;
        }

        public static void AssertGridEqual(Grid want, Grid got)
        {
            Assert.Equal(want.GridCenter, got.GridCenter);
            Assert.Equal(want.GridWorldSize, got.GridWorldSize);
            Assert.Equal(want.MaxSize, got.MaxSize);
            Assert.Equal(want.MapName, got.MapName);

            AssertGridMapEqual(want.MapGrid, got.MapGrid);
            AssertListEntitiesEqual(want.NpcList, want.NpcList);
            AssertListEntitiesEqual(want.ThingList, want.ThingList);
        }

        public static void AssertGridMapEqual(GridNode[,] want, GridNode[,] got)
        {
            Assert.Equal(want.Length, got.Length);
            for (var i = 0; i <= want.GetUpperBound(0); i++)
            {
                for (var j = 0; j <= want.GetUpperBound(1); j++)
                {
                    AssertNodeFromGridsEqual(want, got, i, j);
                }
            }
        }

        private static void AssertNodeFromGridsEqual(GridNode[,] want, GridNode[,] got, int i, int j)
        {
            Assert.Equal(want[i, j].WorldPosition, got[i, j].WorldPosition);
            Assert.Equal(want[i, j].Walkable, got[i, j].Walkable);
            Assert.Equal(want[i, j].Unknown, got[i, j].Unknown);
        }
    }
}