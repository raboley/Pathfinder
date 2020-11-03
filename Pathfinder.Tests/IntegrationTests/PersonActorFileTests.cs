using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Numerics;
using System.Threading.Tasks;
using Pathfinder.Map;
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
            var want = ExampleWorld.ZoneD().Npcs;
            want.Add(new Person(2, "test guy", Vector3.Zero) {MapName = zone.Name});

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = SetupPersister.GetCurrentMethodName();

            var actor = new PersonActor {Persister = persister};
            var watcher = new CollectionWatcher<Person>(zone.Npcs, actor);

            zone.AddNpc(new Person(2, "test guy", Vector3.Zero));
            var got = persister.Load<ObservableCollection<Person>>();

            persister.Delete();

            Assert.Equal(want, got);
        }


        [Fact]
        public async void ConcurrentCallsWillNotLockUpFile()
        {
            var sw = new Stopwatch();

            var zone = ExampleWorld.ZoneD();

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = SetupPersister.GetCurrentMethodName();

            var actor = new PersonActor();
            actor.Persister = persister;

            var watcher = new CollectionWatcher<Person>(zone.Npcs, actor);

            zone.AddNpc(new Person(2, "test guy", Vector3.Zero));
            zone.AddNpc(new Person(2, "test guy", Vector3.Zero));
            zone.AddNpc(new Person(2, "test guy", Vector3.Zero));

            var npcs = new List<Person>();
            for (var i = 0; i < 30; i++) npcs.Add(new Person(i, "dummy" + i, Vector3.Zero));

            await Init(zone, npcs);

            var want = ExampleWorld.ZoneD().Npcs;
            want.Add(new Person(2, "test guy", Vector3.Zero) {MapName = zone.Name});
            for (var i = 0; i < 30; i++) want.Add(new Person(i, "dummy" + i, Vector3.Zero) {MapName = zone.Name});

            var got = persister.Load<ObservableCollection<Person>>();

            // persister.Delete();

            Assert.Equal(want.Count, got.Count);
            Assert.Equal(want, got);
            sw.Stop();
        }

        public async Task<bool> Init(Zone zone, List<Person> npcs)
        {
            var series = npcs;
            var tasks = new List<Task<Tuple<Person, bool>>>();
            foreach (var i in series)
            {
                Console.WriteLine("Starting Process {0}", i);
                tasks.Add(DoWorkAsync(zone, i));
            }

            foreach (var task in await Task.WhenAll(tasks))
                if (task.Item2)
                    Console.WriteLine("Ending Process {0}", task.Item1);
            return true;
        }

        public async Task<Tuple<Person, bool>> DoWorkAsync(Zone zone, Person npc)
        {
            Console.WriteLine("working..{0}", npc);
            zone.AddNpc(npc);
            return Tuple.Create(npc, true);
        }
    }
}