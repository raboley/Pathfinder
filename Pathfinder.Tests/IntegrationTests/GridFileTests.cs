using System.IO;
using Pathfinder.Map;
using Pathfinder.Persistence;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class GridFileTests
    {
        [Fact]
        public void TestCanSaveToFile()
        {
            var want = SetupZoneMap.SetupSmallGrid();
            var mapName = SetupPersister.GetCurrentMethodName();
            var persister = new FilePersister(mapName);

            persister.Save(want);

            var got = persister.Load<ZoneMap>();
            persister.Delete();

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

        [Fact]
        public void TestCanLoadGridFromFile()
        {
            var want = SetupZoneMap.SetupSmallGrid();
            var mapName = SetupPersister.GetCurrentMethodName();
            var persister = new FilePersister(mapName);
            // Path assumes to start from ./debug/ so we want to set it to the test fixtures dir.
            string grandParentDirectory = Directory.GetParent(persister.FilePath).FullName;
            string parentDirectory = Directory.GetParent(grandParentDirectory).FullName;
            persister.FilePath = Path.Combine(parentDirectory, "fixtures");

            // Uncomment to make golden file if zoneMap changes.
            persister.Save(want);

            var got = persister.Load<ZoneMap>();

            SetupZoneMap.AssertGridMapEqual(want.MapGrid, got.MapGrid);
        }

        // Don't run this unless you have time to kill.
        // [Fact]
        // public void PrintOutBastokMap()
        // {
        //     var gridManager = GridFactorySetup.SetupGridFactory();
        //     var zoneMap = gridManager.LoadGrid("Bastok_Mines");
        //
        //     string bastok = zoneMap.PrintKnown();
        //     var persister = SetupPersister.SetupTestFileTextPersister();
        //     persister.MapName = "bastok_map";
        //     persister.DefaultExtension = "txt";
        //     persister.Save(bastok);
        // }
    }
}