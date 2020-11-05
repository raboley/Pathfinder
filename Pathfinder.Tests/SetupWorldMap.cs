using System.Collections.Generic;
using System.Numerics;
using Pathfinder.Map;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests
{
    public class SetupZoneMap
    {
        public static string _smallGridZoneName = "Small ZoneMap";

        public static void AssertPointNotWalkable(ZoneMap zoneMap, Vector3 position)
        {
            var node = zoneMap.GetNodeFromWorldPoint(position);
            Assert.Equal(node.WorldPosition, position);
            Assert.Equal(node.Walkable, false);
        }

        public static ZoneMap SetupSmallGrid()
        {
            var grid = ZoneMap.NewGridFromVector2(new Vector2(3f, 3f));
            grid.MapName = _smallGridZoneName;
            return grid;
        }

        public static ZoneMap SetupMediumGrid()
        {
            var grid = ZoneMap.NewGridFromVector2(new Vector2(5f, 5f));
            return grid;
        }

        public static ZoneMap SetupBigGrid()
        {
            var grid = ZoneMap.NewGridFromVector2(new Vector2(51f, 51f));
            return grid;
        }

        public static List<Node> GetNeighborsListForEdgeNode()
        {
            var want = new List<Node>
            {
                new Node(new Vector3(0, 0, 0)),
                new Node(new Vector3(0, 0, 1)),
                new Node(new Vector3(1, 0, 0))
            };
            // want.Add(new Node(new Vector3(-1, 0, -1)));
            // want.Add(new Node(new Vector3(-1, 0, 0)));
            // want.Add(new Node(new Vector3(-1, 0, 1)));
            // want.Add(new Node(new Vector3(0, 0, -1)));
            // want.Add(new Node(new Vector3(0, 0, 1)));
            // want.Add(new Node(new Vector3(1, 0, -1)));
            // want.Add(new Node(new Vector3(1, 0, 0)));
            // want.Add(new Node(new Vector3(1, 0, 1)));
            return want;
        }

        public static void AssertListGridNodesEqual(List<Node> want, List<Node> got)
        {
            Assert.Equal(want.Count, got.Count);
            for (var i = 0; i < want.Count; i++) Assert.Equal(want[i], got[i]);
        }

        public static void AssertListEntitiesEqual(List<Person> want, List<Person> got)
        {
            Assert.Equal(want.Count, got.Count);

            for (var i = 0; i < want.Count; i++)
            {
                Assert.Equal(want[i].Name, got[i].Name);
                Assert.Equal(want[i].Position, got[i].Position);
                Assert.Equal(want[i].MapName, got[i].MapName);
            }
        }

        public static Node[,] SetupThreeByThreeGrid()
        {
            var want = new Node[3, 3];
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

        public static void AssertGridEqual(ZoneMap want, ZoneMap got)
        {
            Assert.Equal(want.GridCenter, got.GridCenter);
            Assert.Equal(want.GridWorldSize, got.GridWorldSize);
            Assert.Equal(want.MaxSize, got.MaxSize);
            Assert.Equal(want.MapName, got.MapName);

            AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

        public static void AssertGridMapEqual(Node[,] want, Node[,] got)
        {
            Assert.Equal(want.Length, got.Length);
            for (var i = 0; i <= want.GetUpperBound(0); i++)
            for (var j = 0; j <= want.GetUpperBound(1); j++)
                AssertNodeFromGridsEqual(want, got, i, j);
        }

        private static void AssertNodeFromGridsEqual(Node[,] want, Node[,] got, int i, int j)
        {
            Assert.Equal(want[i, j].WorldPosition, got[i, j].WorldPosition);
            Assert.Equal(want[i, j].Walkable, got[i, j].Walkable);
            Assert.Equal(want[i, j].Unknown, got[i, j].Unknown);
        }
    }
}