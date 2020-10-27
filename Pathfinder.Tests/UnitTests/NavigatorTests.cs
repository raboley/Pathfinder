using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NavigatorTests
    {
        [Fact]
        public void NavigatorMoveToWaypointMovesPlayer()
        {
            Vector3[] path = new[]
            {
                new Vector3(-1f),
                new Vector3(0f),
                new Vector3(1f),
            };

            var navigator = new Navigator();

            var traveledPath = navigator.WalkPath(path);

            Assert.Equal(path.Length, traveledPath.Length);
            for (int i = 0; i < path.Length; i++)
            {
                Assert.Equal(path[i], traveledPath[i]);
            }
        }
    }
}