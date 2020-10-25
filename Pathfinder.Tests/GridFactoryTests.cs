using Pathfinder.Pathfinder;
using Xunit;

namespace Pathfinder.Tests.Pathfinder
{
    public class GridFactoryTests
    {
        [Fact]
        public void TestGridFactoryCanGetGridFromFile()
        {
            var want = GridSetup.SetupSmallGrid();
            const string mapName = "Bastok";

            var gridFactory = new GridFactory();
            var persister = PersisterSetup.SetupTestFilePersister();

            gridFactory.Persister = persister;

            // Uncomment to make golden file if grid changes.
            // persister.MapName = mapName;
            // persister.Save(want);

            var got = gridFactory.LoadGrid(mapName);

            GridSetup.AssertGridEqual(want.MapGrid, got.MapGrid);
        }


        // [Fact]
        public void TestGridCreatesNewIfNoFileExists()
        {
            var want = GridSetup.SetupMediumGrid();
            var got = GridSetup.SetupMediumGrid();


            GridSetup.AssertGridEqual(want.MapGrid, got.MapGrid);
        }
    }
}