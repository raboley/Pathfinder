using System;
using System.Numerics;
using System.Threading.Tasks;
using Pathfinder.Pathing;
using Pathfinder.Travel;
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
                new Vector3(2f, 0f, 2f)
            };
            var goal = new Vector3(2f, 0f, 2f);

            var grid = GridSetup.SetupMediumGrid();

            var pathfinding = new Pathfinding();
            pathfinding.Grid = grid;

            var navigator = new Traveler();
            navigator.Position = new Vector3(-2f);
            navigator.Pathfinder = pathfinding;

            var traveledPath = navigator.PathfindAndWalkToFarAwayWorldMapPosition(goal);


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

            var navigator = new Traveler();
            navigator.Position = Vector3.One;
            navigator.Pathfinder = pathfinding;

            navigator.DiscoverAllNodes();


            Assert.Empty(navigator.Pathfinder.Grid.UnknownNodes);

            string got = grid.PrintKnown();
            Assert.Equal(want, got);
        }


        //passes
        [Fact]
        public void TestSuccessResult()
        {
            var testClass = new AsyncTestClass();
            int answer = testClass.AddAsync(1, 1).Result;
            Assert.Equal(2, answer);
        }

        //passes
        [Fact]
        public async void TestSuccessAwait()
        {
            var testClass = new AsyncTestClass();
            int answer = await testClass.AddAsync(1, 1);
            Assert.Equal(2, answer);
        }

        // //fails as expected
        // [Fact]
        // public void TestFailResult()
        // {
        //     var testClass = new AsyncTestClass();
        //     int answer = testClass.AddAsync(2, 1).Result;
        //     Assert.Equal(2, answer);
        // }
        //
        // //fails as expected
        // [Fact]
        // public async void TestFailAwait()
        // {
        //     var testClass = new AsyncTestClass();
        //     int answer = await testClass.AddAsync(2, 1);
        //     Assert.Equal(2, answer);
        // }

        [Fact]
        public async void TestExceptionThrown_Works()
        {
            var testClass = new AsyncTestClass();
            await Assert.ThrowsAsync<ArgumentException>(() => testClass.ErrorAddAsync(0, 1));
        }
    }
}


public class AsyncTestClass
{
    public async Task<int> AddAsync(int x, int y)
    {
        return await Task.Run(() => x + y);
    }

    public async Task<int> ErrorAddAsync(int x, int y)
    {
        if (x == 0) throw new ArgumentException("zero");
        return await Task.Run(() => x + y);
    }
}