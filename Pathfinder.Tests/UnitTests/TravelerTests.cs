using System;
using System.Numerics;
using System.Threading.Tasks;
using Pathfinder.Map;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class NavigatorTests
    {
        [Fact]
        public void NavigatorFindsPathToWaypoint()
        {
            var startPos = new Vector3(-2, 0, -2);
            var endPos = new Vector3(2, 0, 2);
            Vector3[] want =
            {
                startPos,
                new Vector3(1f, 0f, 1f),
                endPos
            };

            var zone = new Zone();
            var grid = SetupZoneMap.SetupMediumGrid();
            zone.Map = grid;

            var traveler = new Traveler {Position = startPos, CurrentZone = zone};


            var traveledPath = traveler.PathfindAndWalkToFarAwayWorldMapPosition(endPos);


            Assert.Equal(endPos, traveler.Position);
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
            var grid = SetupZoneMap.SetupSmallGrid();
            var traveler = new Traveler();
            traveler.CurrentZone = new Zone {Map = grid};
            traveler.Position = Vector3.One;

            traveler.DiscoverAllNodes();


            Assert.Empty(traveler.CurrentZone.Map.UnknownNodes);

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