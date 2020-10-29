using System.Numerics;
using Pathfinder.WorldMap;
using Xunit;

namespace Pathfinder.Tests.IntegrationTests
{
    public class WorldMapNodeFileTests
    {
        [Fact]
        public void TestCanSaveToFile()
        {
            var want = new WorldMapNode(Vector3.One);
            var persister = SetupPersister.SetupTestFilePersister();

            persister.Save(want);

            WorldMapNode got;
            try
            {
                got = persister.Load<WorldMapNode>();
            }
            finally
            {
                persister.Delete();
            }

            Assert.Equal(want.WorldPosition, got.WorldPosition);
        }
    }
}