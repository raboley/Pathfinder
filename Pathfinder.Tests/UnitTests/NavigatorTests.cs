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
                new Vector3(1f, 0f, 1f),
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
            for (int i = 0; i < want.Length; i++)
            {
                Assert.Equal(want[i], traveledPath[i]);
            }
        }
    }
}