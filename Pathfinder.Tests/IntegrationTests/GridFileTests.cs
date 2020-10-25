using System.IO;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class GridFileTests
    {
        [Fact]
        public void TestCanLoadGridFromFile()
        {
            var want = GridSetup.SetupSmallGrid();
            var persister = new FilePersister("TestCanLoadGridFromFile.golden");
            // Path assumes to start from ./debug/ so we want to set it to the test fixtures dir.
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");

            // Uncomment to make golden file if grid changes.
            // persister.Save(want);

            var got = persister.Load<Grid>();

            GridSetup.AssertGridEqual(want.MapGrid, got.MapGrid);
        }
    }
}