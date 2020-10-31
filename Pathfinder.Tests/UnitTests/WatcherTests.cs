using System.Numerics;
using Pathfinder.Travel;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WatcherTests
    {
        [Fact]
        public void WatcherCanWatchTraveler()
        {
            var traveler = new Traveler();
            var spyActor = new SpyActor();
            var watcher = new Watcher(traveler, spyActor);

            traveler.Position = Vector3.One;

            const int want = 1;
            int got = spyActor.CalledTimesUpdate;

            Assert.Equal(want, got);
        }
    }
}