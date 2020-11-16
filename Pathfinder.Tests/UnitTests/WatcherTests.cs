using System.Linq;
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

            traveler.CurrentZone = World.FinalFantasy().Zones.FirstOrDefault();
            traveler.Walker.CurrentPosition = Vector3.One;


            const int want = 1;
            int got = spyActor.CalledTimesUpdate;

            Assert.Equal(want, got);
        }
    }
}