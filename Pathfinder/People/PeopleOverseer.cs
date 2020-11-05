using System.IO;
using Pathfinder.Persistence;

namespace Pathfinder.People
{
    public class PeopleOverseer
    {
        public PeopleOverseer(string mapName, string directory = "NPCs")
        {
            PeopleManager = new PeopleManager(mapName);
            Persister = SetupPersonPersister(mapName, directory);

            PeopleManager.LoadPeopleOrCreateNew(Persister);

            var actor = new PersonActor {Persister = Persister};
            var watcher = new CollectionWatcher<Person>(PeopleManager.People, actor);
        }

        public PeopleManager PeopleManager { get; set; }
        public IPersister Persister { get; set; }

        public static FilePersister SetupPersonPersister(string mapName, string directory)
        {
            var persister = new FilePersister();
            persister.DefaultExtension = "json";
            string greatGrandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string grandParentDirectory = Directory.GetParent(greatGrandParentDirectory).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, directory);
            persister.FileName = mapName;
            return persister;
        }
    }
}