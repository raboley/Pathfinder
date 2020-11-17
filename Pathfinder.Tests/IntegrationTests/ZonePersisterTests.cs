using System.Linq;
using Pathfinder.Map;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class ZonePersisterTests
    {
        [Fact]
        public void FilePersisterCanSaveAndLoadZone()
        {
            var want = World.FinalFantasy().Zones.FirstOrDefault();
            var persister = SetupPersister.SetupTestFilePersister();
            string mapName = SetupPersister.GetCurrentMethodName();
            persister.FileName = mapName;

            persister.Save(want);
            var got = persister.Load<Zone>();
            persister.Delete();

            Assert.Equal(want, got);
        }
    }
}