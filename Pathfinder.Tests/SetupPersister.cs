using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Pathfinder.Persistence;

namespace Pathfinder.Tests
{
    public class SetupPersister
    {
        public static FilePersister SetupTestFilePersister()
        {
            var persister = new FilePersister();
            persister.DefaultExtension = "json";
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");
            return persister;
        }

        public static FileTextPersister SetupTestFileTextPersister()
        {
            var persister = new FileTextPersister();
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");
            persister.FileName = GetCurrentMethodName();
            return persister;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethodName()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
    }
}