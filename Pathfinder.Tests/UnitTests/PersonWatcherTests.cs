using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class PersonWatcherTests
    {
        [Fact]
        public void AddingPersonTriggersAdd()
        {
            var want = 1;
            var zone = ExampleWorld.ZoneD();
            var actor = new SpyActor();
            var watcher = new CollectionWatcher<Person>(zone.Npcs, actor);

            zone.Npcs.Add(new Person(2, "test guy", Vector3.Zero));


            int got = actor.CalledTimesAdd;

            Assert.Equal(want, got);
        }
    }
}