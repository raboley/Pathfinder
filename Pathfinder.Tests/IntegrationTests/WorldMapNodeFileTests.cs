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
            var want = new Node(Vector3.One);
            var persister = SetupPersister.SetupTestFilePersister();

            persister.Save(want);

            Node got;
            try
            {
                got = persister.Load<Node>();
            }
            finally
            {
                persister.Delete();
            }

            Assert.Equal(want.WorldPosition, got.WorldPosition);
        }
    }
}