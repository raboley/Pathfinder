using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NavigatorTests
    {
        [Fact]
        public void NavigatorFindsPathToWaypoint()
        {
            Vector3[] want =
            {
                new Vector3(1f, 0f, 1f)
            };
            var goal = new Vector3(2f);

            var grid = GridSetup.SetupMediumGrid();

            var pathfinding = new Pathfinding();
            pathfinding.Grid = grid;

            var navigator = new Navigator();
            navigator.Position = new Vector3(-2f);
            navigator.Pathfinder = pathfinding;

            var traveledPath = navigator.WalkToWaypoint(goal);


            Assert.Equal(goal, navigator.Position);
            Assert.Equal(want.Length, traveledPath.Length);
            for (var i = 0; i < want.Length; i++) Assert.Equal(want[i], traveledPath[i]);
        }

        [Fact]
        public void NavigatorDiscoversAllUnknownNodes()
        {
            const string want = @"
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
|     |     |     |
-------------------
";
            var grid = GridSetup.SetupSmallGrid();
            var pathfinding = new Pathfinding();
            pathfinding.Grid = grid;

            var navigator = new Navigator();
            navigator.Position = Vector3.One;
            navigator.Pathfinder = pathfinding;

            navigator.DiscoverAllNodes();


            Assert.Empty(navigator.Pathfinder.Grid.UnknownNodes);

            string got = grid.PrintKnown();
            Assert.Equal(want, got);
        }
    }
}