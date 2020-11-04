using System.IO;
using Pathfinder.Persistence;

namespace Pathfinder.People
{
    public class PeopleOverseer
    {
        public PeopleOverseer(string mapName, string directory = "NPCs")
        {
            PeopleManager = new PeopleManager(mapName);
            var persister = SetupPersonPersister(mapName, directory);

            PeopleManager.LoadPeopleOrCreateNew(persister);

            var actor = new PersonActor {Persister = persister};
            var watcher = new CollectionWatcher<Person>(PeopleManager.People, actor);
        }

        public PeopleManager PeopleManager { get; set; }
        public IPersister Persister { get; set; }

        public static FilePersister SetupPersonPersister(string mapName, string directory)
        {
            var persister = new FilePersister();
            persister.DefaultExtension = "json";
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, directory);
            persister.FileName = mapName;
            return persister;
        }
    }
}