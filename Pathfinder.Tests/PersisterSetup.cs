using System.IO;

namespace Pathfinder.Tests
{
    public class PersisterSetup
    {
        public static FilePersister SetupTestFilePersister()
        {
            var persister = new FilePersister();
            persister.DefaultExtension = "golden";
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");
            return persister;
        }
    }
}