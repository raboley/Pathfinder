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
        private const string testFileDirectory = "fixtures";

        [Fact]
        public void AddingPersonPersistsCollectionToFile()
        {
            var mapName = SetupPersister.GetCurrentMethodName();
            var want = new ObservableCollection<Person>();
            want.Add(new Person(2, "test guy", Vector3.Zero) {MapName = mapName});

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = mapName;

            var peopleOverseer = new PeopleOverseer(mapName, testFileDirectory);

            peopleOverseer.PeopleManager.AddPerson(new Person(2, "test guy", Vector3.Zero));
            var got = persister.Load<ObservableCollection<Person>>();

            persister.Delete();

            Assert.Equal(want, got);
        }

        [Fact]
        public void WillGetNpcsFromFileIfExists()
        {
            var mapName = SetupPersister.GetCurrentMethodName();
            var want = new ObservableCollection<Person>();
            want.Add(new Person(2, "test guy", Vector3.Zero) {MapName = mapName});

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = SetupPersister.GetCurrentMethodName();

            var peopleOverseer = new PeopleOverseer(mapName, testFileDirectory);

            peopleOverseer.PeopleManager.AddPerson(new Person(2, "test guy", Vector3.Zero));

            // Load up using what was just saved.
            var gotZone = new Zone("D");
            gotZone.Npcs = persister.Load<ObservableCollection<Person>>();

            var got = gotZone.Npcs;

            persister.Delete();

            Assert.Equal(want, got);
        }


        [Fact]
        public async void ConcurrentCallsWillNotLockUpFile()
        {
            var sw = new Stopwatch();
            var mapName = SetupPersister.GetCurrentMethodName();

            var persister = SetupPersister.SetupTestFilePersister();
            persister.FileName = SetupPersister.GetCurrentMethodName();

            var peopleOverseer = new PeopleOverseer(mapName, testFileDirectory);

            peopleOverseer.PeopleManager.AddPerson(new Person(0, "test guy", Vector3.Zero));
            peopleOverseer.PeopleManager.AddPerson(new Person(0, "test guy", Vector3.Zero));
            peopleOverseer.PeopleManager.AddPerson(new Person(0, "test guy", Vector3.Zero));

            var npcs = new List<Person>();
            for (var i = 2; i < 30; i++)
                peopleOverseer.PeopleManager.AddPerson(new Person(i, "dummy" + i, Vector3.Zero));

            await Init(peopleOverseer.PeopleManager, npcs);

            var want = new ObservableCollection<Person>();
            want.Add(new Person(0, "test guy", Vector3.Zero) {MapName = mapName});
            for (var i = 2; i < 30; i++) want.Add(new Person(i, "dummy" + i, Vector3.Zero) {MapName = mapName});

            var got = persister.Load<ObservableCollection<Person>>();

            persister.Delete();

            Assert.Equal(want.Count, got.Count);
            Assert.Equal(want, got);
            sw.Stop();
        }

        public async Task<bool> Init(PeopleManager peopleManager, List<Person> npcs)
        {
            var series = npcs;
            var tasks = new List<Task<Tuple<Person, bool>>>();
            foreach (var i in series)
            {
                Console.WriteLine("Starting Process {0}", i);
                tasks.Add(DoWorkAsync(peopleManager, i));
            }

            foreach (var task in await Task.WhenAll(tasks))
                if (task.Item2)
                    Console.WriteLine("Ending Process {0}", task.Item1);
            return true;
        }

        public async Task<Tuple<Person, bool>> DoWorkAsync(PeopleManager peopleManager, Person npc)
        {
            Console.WriteLine("working..{0}", npc);
            peopleManager.AddPerson(npc);
            return Tuple.Create(npc, true);
        }
    }
}