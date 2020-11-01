using System.IO;
using System.Numerics;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class GridFactoryFileTests
    {
        [Fact]
        public void TestGridFactoryCanGetGridFromFile()
        {
            var want = SetupZoneMap.SetupSmallGrid();
            var gridFactory = GridFactorySetup.SetupGridFactory();
            string mapName = SetupPersister.GetCurrentMethodName();

            // Uncomment to make golden file if zoneMap changes.
            gridFactory.Persister.MapName = mapName;
            gridFactory.Persister.Save(want);

            var got = gridFactory.LoadGrid(mapName);

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

        [Fact]
        public void TestLoadGridFailsIfFileNotExists()
        {
            var want = SetupZoneMap.SetupMediumGrid();
            var gridFactory = GridFactorySetup.SetupGridFactory();

            Assert.Throws<FileNotFoundException>(() => gridFactory.LoadGrid(Path.GetRandomFileName()));
        }

        [Fact]
        public void LoadGridOrCreateNewLoadsGridIfExists()
        {
            var want = SetupZoneMap.SetupSmallGrid();
            var gridFactory = GridFactorySetup.SetupGridFactory();
            string mapName = SetupPersister.GetCurrentMethodName();
            want.MapName = mapName;

            // Uncomment to make golden file if zoneMap changes.
            gridFactory.Persister.MapName = mapName;
            gridFactory.Persister.Save(want);

            var got = gridFactory.LoadGridOrCreateNew(mapName);

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
            Assert.Equal(want.MapName, got.MapName);
        }

        [Fact]
        public void LoadGridOrCreateNewCreatesNewIfItDoesntExist()
        {
            var want = SetupZoneMap.SetupMediumGrid();
            var gridFactory = GridFactorySetup.SetupGridFactory();
            gridFactory.DefaultGridSize = new Vector2(5f, 5f);

            string mapName = Path.GetRandomFileName();
            want.MapName = mapName;

            var got = gridFactory.LoadGridOrCreateNew(mapName);

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
            Assert.Equal(want.MapName, got.MapName);
        }
    }
}