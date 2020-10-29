using System.Collections.ObjectModel;
using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.UnitTests
{
    public class WatcherTests
    {
        [Fact]
        public void WatcherTakesInCollection()
        {
            var want = new ObservableCollection<string> {"one", "two", "three"};

            var watcher = new Watcher<string> {Collection = want};

            for (var i = 0; i < want.Count; i++) Assert.Equal(want[i], watcher.Collection[i]);
        }

        [Fact]
        public void WatcherFiresAddedWhenCollectionGetsElementAdded()
        {
            var want = new ObservableCollection<string> {"one", "two", "three"};
            var spyActor = new SpyActor();
            want.CollectionChanged += spyActor.Changed;

            var watcher = new Watcher<string> {Collection = want, Actor = spyActor};

            want.Add("Four");

            Assert.Equal(1, spyActor.CalledTimesAdd);
        }

        [Fact]
        public void WatcherFiresRemovedWhenCollectionGetsElementRemoved()
        {
            var want = new ObservableCollection<string> {"one", "two", "three"};
            var spyActor = new SpyActor();
            want.CollectionChanged += spyActor.Changed;

            var watcher = new Watcher<string> {Collection = want, Actor = spyActor};

            want.RemoveAt(0);

            Assert.Equal(1, spyActor.CalledTimesRemove);
        }

        [Fact]
        public void WatcherFiresUpdatedWhenObjectUpdatedInPlace()
        {
            var people = PeopleManagerTests.SetupPeopleCollection();
            var spyActor = new SpyActor();
            people.CollectionChanged += spyActor.Changed;
            var peopleManager = new PeopleManager {People = people};

            var watcher = new Watcher<Person> {Collection = peopleManager.People, Actor = spyActor};

            peopleManager.AddOrUpdatePerson(new Person(1, "Jim", Vector3.Zero));

            Assert.Equal(1, spyActor.CalledTimesUpdate);
            Assert.Equal(0, spyActor.CalledTimesRemove);
            Assert.Equal(0, spyActor.CalledTimesAdd);
        }
    }
}