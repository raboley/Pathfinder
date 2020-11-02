using System.Collections.ObjectModel;
using System.Numerics;
using Pathfinder.People;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class PersonActorFileTests
    {
        [Fact]
        public void AddingPersonPersistsCollectionToFile()
        {
            var zone = ExampleWorld.ZoneD();

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = SetupPersister.GetCurrentMethodName();

            var actor = new PersonActor();
            actor.Persister = persister;

            var watcher = new CollectionWatcher<Person>(zone.Npcs, actor);

            zone.Npcs.Add(new Person(2, "test guy", Vector3.Zero));

            var want = ExampleWorld.ZoneD().Npcs;
            want.Add(new Person(2, "test guy", Vector3.Zero));

            var got = persister.Load<ObservableCollection<Person>>();

            persister.Delete();

            Assert.Equal(want, got);
        }
    }
}