using System.IO;
using Pathfinder.Persistence;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class GridFileTests
    {
        [Fact]
        public void TestCanSaveToFile()
        {
            var want = GridSetup.SetupSmallGrid();
            var persister = new FilePersister("tempGrid.golden");

            persister.Save(want);

            var got = persister.Load<Grid>();
            persister.Delete();

            GridSetup.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

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
            persister.Save(want);

            var got = persister.Load<Grid>();

            GridSetup.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

        // Don't run this unless you have time to kill.
        // [Fact]
        // public void PrintOutBastokMap()
        // {
        //     var gridManager = GridFactorySetup.SetupGridFactory();
        //     var grid = gridManager.LoadGrid("Bastok_Mines");
        //
        //     string bastok = grid.PrintKnown();
        //     var persister = PersisterSetup.SetupTestFileTextPersister();
        //     persister.MapName = "bastok_map";
        //     persister.DefaultExtension = "txt";
        //     persister.Save(bastok);
        // }
    }
}