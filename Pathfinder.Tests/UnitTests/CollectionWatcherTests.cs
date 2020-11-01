using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class CollectionWatcherTests
    {
        [Fact]
        public void WatcherTakesInCollection()
        {
            var peopleCollection = PeopleManagerTests.SetupPeopleCollection();
            var spyActor = new SpyActor();
            var peopleManager = new PeopleManager {People = peopleCollection};
            var watcher = new CollectionWatcher<Person>(peopleManager.People, spyActor);
            var want = PeopleManagerTests.SetupPeopleCollection();

            for (var i = 0; i < want.Count; i++) Assert.Equal(want[i], watcher.Collection[i]);
        }

        [Fact]
        public void WatcherFiresAddedWhenCollectionGetsElementAdded()
        {
            var spyActor = SetupWatcher(out var peopleManager);
            peopleManager.People.Add(new Person(99, "new guy", Vector3.Zero));

            Assert.Equal(1, spyActor.CalledTimesAdd);
        }

        [Fact]
        public void WatcherFiresRemovedWhenCollectionGetsElementRemoved()
        {
            var spyActor = SetupWatcher(out var peopleManager);
            peopleManager.People.RemoveAt(0);

            Assert.Equal(1, spyActor.CalledTimesRemove);
        }

        [Fact]
        public void WatcherFiresUpdatedWhenObjectUpdatedInPlace()
        {
            var spyActor = SetupWatcher(out var peopleManager);

            var want = new Person(1, "Jim", Vector3.One);
            peopleManager.AddOrUpdatePerson(want);
            var got = peopleManager.GetPerson(want);

            Assert.Equal(want, got);
            Assert.Equal(0, spyActor.CalledTimesRemove);
            Assert.Equal(0, spyActor.CalledTimesAdd);
            Assert.Equal(1, spyActor.CalledTimesUpdate);
        }

        private static SpyActor SetupWatcher(out PeopleManager peopleManager)
        {
            var peopleCollection = PeopleManagerTests.SetupPeopleCollection();
            var spyActor = new SpyActor();
            peopleManager = new PeopleManager {People = peopleCollection};
            var watcher = new CollectionWatcher<Person>(peopleManager.People, spyActor);
            return spyActor;
        }
    }
}